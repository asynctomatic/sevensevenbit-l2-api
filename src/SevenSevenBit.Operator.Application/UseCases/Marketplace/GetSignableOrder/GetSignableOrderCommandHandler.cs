namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.GetSignableOrder;

using System.Numerics;
using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Exceptions;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects.Signable;
using SharedKernel.Extensions;

public class GetSignableOrderCommandHandler : IRequestHandler<GetSignableOrderCommand, Result<SignableOrder>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly INonceService nonceService;
    private readonly ITimestampService timestampService;
    private readonly IVaultService vaultService;
    private readonly IStarkExEncodingService starkExEncodingService;

    public GetSignableOrderCommandHandler(
        IUnitOfWork unitOfWork,
        INonceService nonceService,
        ITimestampService timestampService,
        IVaultService vaultService,
        IStarkExEncodingService starkExEncodingService)
    {
        this.unitOfWork = unitOfWork;
        this.nonceService = nonceService;
        this.timestampService = timestampService;
        this.vaultService = vaultService;
        this.starkExEncodingService = starkExEncodingService;
    }

    public async Task<Result<SignableOrder>> Handle(GetSignableOrderCommand command, CancellationToken cancellationToken)
    {
        var marketplace = await unitOfWork.Repository<Marketplace>().GetByIdAsync(
            command.MarketplaceId, cancellationToken);
        if (marketplace is null)
        {
            // TODO: log error.
            return new Result<SignableOrder>(new MarketplaceNotFoundException(command.MarketplaceId));
        }

        var user = await unitOfWork.Repository<User>().GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            // TODO: log error.
            return new Result<SignableOrder>(new UserNotFoundException(command.UserId));
        }

        var baseAssetAmountQuantized = command.BaseAssetAmount / marketplace.BaseAsset.Quantum.Value;
        var quoteAssetAmountQuantized = command.QuoteAssetAmount / marketplace.QuoteAsset.Quantum.Value;
        if (
            baseAssetAmountQuantized == BigInteger.Zero ||
            quoteAssetAmountQuantized == BigInteger.Zero ||
            baseAssetAmountQuantized / quoteAssetAmountQuantized == BigInteger.Zero)
        {
            // TODO: log error.
            // TODO: InvalidOperationAmountException.
        }

        // Validate user balance.
        var senderVault = user.Vaults.Find(v => v.AssetId.Equals(
            command.Side == OrderSide.Bid ? marketplace.QuoteAssetId : marketplace.BaseAssetId));
        if (senderVault is null || senderVault.QuantizedAvailableBalance <
            (command.Side == OrderSide.Bid ? quoteAssetAmountQuantized : baseAssetAmountQuantized))
        {
            // TODO: log error.
            // TODO: return Result.Failure<MarketplaceOrderDto>(new Error("Insufficient balance"));
        }

        // Allocate a new vault if the user does not have a vault for the received asset.
        var receiverVault = user.Vaults.Find(v => v.AssetId.Equals(
            command.Side == OrderSide.Bid ? marketplace.BaseAssetId : marketplace.QuoteAssetId));
        receiverVault ??= await vaultService.AllocateVaultAsync(
            user,
            command.Side == OrderSide.Bid ? marketplace.BaseAsset : marketplace.QuoteAsset,
            cancellationToken,
            command.Side == OrderSide.Bid ? marketplace.BaseAssetTokenId : marketplace.QuoteAssetTokenId);

        // Calculate expiration timestamp.
        var expirationTimestamp = timestampService.GetTargetExpirationTimestamp();

        // Calculate order nonce.
        var nonce = nonceService.GetRandomNonce();

        // Compute signable order.
        var signableOrder = starkExEncodingService.EncodeLimitOrder(
            senderVault,
            receiverVault.VaultChainId,
            receiverVault.AssetStarkExId(),
            command.Side == OrderSide.Bid ? quoteAssetAmountQuantized : baseAssetAmountQuantized,
            command.Side == OrderSide.Bid ? baseAssetAmountQuantized : quoteAssetAmountQuantized,
            expirationTimestamp,
            nonce,
            null,
            null);

        return new Result<SignableOrder>(new SignableOrder(
            baseAssetAmountQuantized,
            quoteAssetAmountQuantized,
            nonce,
            expirationTimestamp,
            signableOrder));
    }
}
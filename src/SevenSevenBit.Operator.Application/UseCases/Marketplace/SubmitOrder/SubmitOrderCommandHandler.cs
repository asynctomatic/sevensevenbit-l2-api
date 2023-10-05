namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.SubmitOrder;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Contracts.Commands;
using SevenSevenBit.Operator.Application.Interfaces.Services.Signatures;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Client.SDK.Enums.Spot;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;
using StarkEx.Commons.SDK.Models;

public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, Result<MarketplaceOrder>>
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMessageBusService messageBus;
    private readonly IStarkExEncodingService starkExEncodingService;
    private readonly IStarkExSignatureService starkExSignatureService;

    public SubmitOrderCommandHandler(
        IUnitOfWork unitOfWork,
        IMessageBusService messageBus,
        IStarkExEncodingService starkExEncodingService,
        IStarkExSignatureService starkExSignatureService)
    {
        this.unitOfWork = unitOfWork;
        this.messageBus = messageBus;
        this.starkExEncodingService = starkExEncodingService;
        this.starkExSignatureService = starkExSignatureService;
    }

    public async Task<Result<MarketplaceOrder>> Handle(SubmitOrderCommand command, CancellationToken cancellationToken)
    {
        var marketplace = await unitOfWork.Repository<Marketplace>().GetByIdAsync(
            command.MarketplaceId, cancellationToken);
        if (marketplace is null)
        {
            // TODO: log error.
            // TODO: return Result.Failure<MarketplaceOrderDto>(new Error("Markeplace not found"));
        }

        var user = await unitOfWork.Repository<User>().GetByIdAsync(command.UserId, cancellationToken);
        if (user is null)
        {
            // TODO: log error.
            // TODO: return Result.Failure<MarketplaceOrderDto>(new Error("User not found"));
        }

        // Validate user balance.
        var senderVault = user.Vaults.Find(v => v.AssetId.Equals(
            command.Side == OrderSide.Bid ? marketplace.QuoteAssetId : marketplace.BaseAssetId));
        if (senderVault is null || senderVault.QuantizedAvailableBalance <
            (command.Side == OrderSide.Bid ? command.QuoteAssetAmountQuantized : command.BaseAssetAmountQuantized))
        {
            // TODO: log error.
            // TODO: return Result.Failure<MarketplaceOrderDto>(new Error("Insufficient balance"));
        }

        var receiverVault = user.Vaults.Find(v => v.AssetId.Equals(
            command.Side == OrderSide.Bid ? marketplace.BaseAssetId : marketplace.QuoteAssetId));
        if (receiverVault is null)
        {
            // TODO: log error.
            // TODO: return Result.Failure<MarketplaceOrderDto>(new Error("Receiver vault not found"));
        }

        // Validate order signature.
        // TODO: EncodeLimitOrder should receive a receiver vault as argument.
        var signableOrder = starkExEncodingService.EncodeLimitOrder(
            senderVault,
            receiverVault.VaultChainId, // TODO: receiver vault.
            receiverVault.AssetStarkExId(),   // TODO: receiverAssetId
            command.Side == OrderSide.Bid ? command.QuoteAssetAmountQuantized : command.BaseAssetAmountQuantized,
            command.Side == OrderSide.Bid ? command.BaseAssetAmountQuantized : command.QuoteAssetAmountQuantized,
            command.ExpirationTimestamp,
            command.Nonce,
            null,
            null);
        if (!starkExSignatureService.ValidateStarkExSignature(signableOrder, user.StarkKey, command.Signature))
        {
            // TODO: log error.
            // TODO: return Result.Failure<MarketplaceOrderDto>(new Error("Invalid signature"));
        }

        // Create preliminary order.
        var order = new MarketplaceOrder
        {
            Marketplace = marketplace,
            User = user,
            Type = OrderType.Limit,
            Side = command.Side,
            Status = OrderStatus.Placed,
            Matches = new List<OrderMatch>(),
            OrderModel = new OrderRequestModel
            {
                BuyAmount = command.Side == OrderSide.Bid ? command.BaseAssetAmountQuantized : command.QuoteAssetAmountQuantized,
                SellAmount = command.Side == OrderSide.Bid ? command.QuoteAssetAmountQuantized : command.BaseAssetAmountQuantized,
                ExpirationTimestamp = command.ExpirationTimestamp,
                FeeInfo = new FeeInfoModel(),
                Nonce = command.Nonce,
                PublicKey = user.StarkKey.Value,
                Signature = new SignatureModel
                {
                    R = command.Signature.R,
                    S = command.Signature.S,
                },
                TokenBuy = receiverVault.AssetStarkExId(),
                TokenSell = senderVault.AssetStarkExId(),
                Type = SpotOrderRequestType.OrderL2Request,
                VaultIdBuy = receiverVault.VaultChainId.Value,
                VaultIdSell = senderVault.VaultChainId.Value,
            },
        };

        // Pre-Matching steps:
        // 0. Reserve full order amount.
        senderVault.QuantizedAvailableBalance -= command.Side == OrderSide.Bid ?
            command.QuoteAssetAmountQuantized : command.BaseAssetAmountQuantized;
        senderVault.QuantizedAccountingBalance -= command.Side == OrderSide.Bid ?
            command.QuoteAssetAmountQuantized : command.BaseAssetAmountQuantized;

        // Matching steps:
        // 1. Calculate price.
        // This is order.Price;
        // 2. Get all orders on the opposite side until they preface size an the price.
        // TODO: Only get orders that are not fully filled or cancelled.
        var matchableOrders = marketplace.Orders.Where(o => o.Side != order.Side && o.Price <= order.Price)
            .OrderBy(o => o.Price).ThenBy(o => o.CreatedAt).ToList();

        // 3. For all fetched order fill them fully  until the last.
        foreach (var matchableOrder in matchableOrders)
        {
            var matchResult = matchableOrder.MatchOrder(order);
            matchResult.Match(
                Some: async orderMatch =>
                {
                    // Handle the case where a match was made.
                    Console.WriteLine($"Matched: {orderMatch.Quantity} at {orderMatch.Price}");

                    // Send StarkEx settlement transaction for each match.
                    var streamMessage = new StreamTransaction
                    {
                        TransactionStreamId = orderMatch.Transaction.Id,
                        // TODO: StarkExInstanceId = tenantContext.StarkExInstanceDetails.StarkExInstanceId,
                        // TODO: StarkExInstanceBaseAddress = tenantContext.StarkExInstanceDetails.StarkExInstanceHost,
                        // TODO: StarkExInstanceApiVersion = tenantContext.StarkExInstanceDetails.StarkExInstanceApiVersion,
                        Transaction = orderMatch.Transaction.RawTransaction.ToJson(),
                    };
                    await messageBus.Publish(streamMessage, cancellationToken);
                },
                None: () =>
                {
                    // Handle the case where no match was made.
                    Console.WriteLine("No match.");
                });

            if (order.Status == OrderStatus.Filled)
            {
                // Order is fully filled.
                break;
            }
        }

        await unitOfWork.Repository<MarketplaceOrder>().InsertAsync(order, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        return new Result<MarketplaceOrder>(order);
    }
}
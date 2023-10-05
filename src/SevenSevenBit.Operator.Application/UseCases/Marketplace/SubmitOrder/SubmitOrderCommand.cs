namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.SubmitOrder;

using System.Numerics;
using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;

public record SubmitOrderCommand(
    Guid MarketplaceId,
    Guid UserId,
    OrderSide Side,
    BigInteger BaseAssetAmountQuantized,
    BigInteger QuoteAssetAmountQuantized,
    int Nonce,
    long ExpirationTimestamp,
    StarkSignature Signature) : IRequest<Result<MarketplaceOrder>>;
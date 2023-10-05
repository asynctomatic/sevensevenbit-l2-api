namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.GetSignableOrder;

using System.Numerics;
using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects.Signable;

public record GetSignableOrderCommand(
    Guid MarketplaceId,
    Guid UserId,
    OrderSide Side,
    BigInteger BaseAssetAmount,
    BigInteger QuoteAssetAmount) : IRequest<Result<SignableOrder>>;
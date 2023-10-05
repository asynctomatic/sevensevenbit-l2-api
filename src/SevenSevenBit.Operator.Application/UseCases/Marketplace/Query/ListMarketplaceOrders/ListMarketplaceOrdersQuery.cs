namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.ListMarketplaceOrders;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;

public record ListMarketplaceOrdersQuery(Guid MarketplaceId, OrderSide Side, bool IncludeInactive = false)
    : IRequest<Result<IEnumerable<MarketplaceOrder>>>;
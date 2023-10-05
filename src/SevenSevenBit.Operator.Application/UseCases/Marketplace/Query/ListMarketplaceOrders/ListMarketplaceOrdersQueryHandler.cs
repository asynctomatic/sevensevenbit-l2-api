namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.ListMarketplaceOrders;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public class ListMarketplaceOrdersQueryHandler
    : IRequestHandler<ListMarketplaceOrdersQuery, Result<IEnumerable<MarketplaceOrder>>>
{
    private readonly IUnitOfWork unitOfWork;

    public ListMarketplaceOrdersQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<MarketplaceOrder>>> Handle(ListMarketplaceOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await unitOfWork.Repository<MarketplaceOrder>().GetAsync(
            filter: order =>
                order.MarketplaceId == query.MarketplaceId &&
                order.Side == query.Side &&
                (query.IncludeInactive || order.IsActive()),
            cancellationToken: cancellationToken);

        return new Result<IEnumerable<MarketplaceOrder>>(orders);
    }
}
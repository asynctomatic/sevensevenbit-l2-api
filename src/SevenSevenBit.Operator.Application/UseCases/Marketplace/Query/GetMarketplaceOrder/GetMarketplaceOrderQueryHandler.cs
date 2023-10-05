namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.GetMarketplaceOrder;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Exceptions;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public class GetMarketplaceOrderQueryHandler
    : IRequestHandler<GetMarketplaceOrderQuery, Result<MarketplaceOrder>>
{
    private readonly IUnitOfWork unitOfWork;

    public GetMarketplaceOrderQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<MarketplaceOrder>> Handle(GetMarketplaceOrderQuery query, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Repository<MarketplaceOrder>().GetByIdAsync(
            query.OrderId, cancellationToken);
        if (order is null || order.MarketplaceId != query.MarketplaceId)
        {
            // TODO: log error.
            return new Result<MarketplaceOrder>(new OrderNotFoundException(query.OrderId));
        }

        return new Result<MarketplaceOrder>(order);
    }
}
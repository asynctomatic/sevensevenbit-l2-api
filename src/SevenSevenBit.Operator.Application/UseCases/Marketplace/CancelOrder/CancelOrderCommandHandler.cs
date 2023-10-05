namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.CancelOrder;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Exceptions;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;

public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result<MarketplaceOrder>>
{
    private readonly IUnitOfWork unitOfWork;

    public CancelOrderCommandHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<MarketplaceOrder>> Handle(CancelOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.Repository<MarketplaceOrder>().GetByIdAsync(
            command.OrderId, cancellationToken);
        if (order is null || order.MarketplaceId != command.MarketplaceId)
        {
            // TODO: log error.
            return new Result<MarketplaceOrder>(new OrderNotFoundException(command.OrderId));
        }

        if (order.IsActive())
        {
            // TODO: log error.
            // TODO: return Result.Failure<MarketplaceOrderDto>(new Error("Order already cancelled"));
        }

        order.Status = OrderStatus.Cancelled;

        await unitOfWork.SaveAsync(cancellationToken);

        return order;
    }
}
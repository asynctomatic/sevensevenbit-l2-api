namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.GetMarketplace;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Exceptions;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public class GetMarketplaceQueryHandler : IRequestHandler<GetMarketplaceQuery, Result<Marketplace>>
{
    private readonly IUnitOfWork unitOfWork;

    public GetMarketplaceQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<Marketplace>> Handle(GetMarketplaceQuery query, CancellationToken cancellationToken)
    {
        var marketplace = await unitOfWork.Repository<Marketplace>().GetByIdAsync(query.MarketplaceId, cancellationToken);
        if (marketplace is null)
        {
            // TODO: log error.
            return new Result<Marketplace>(new MarketplaceNotFoundException(query.MarketplaceId));
        }

        return new Result<Marketplace>(marketplace);
    }
}
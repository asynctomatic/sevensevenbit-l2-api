namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.ListMarketplaces;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public class GetAllMarketplacesQueryHandler : IRequestHandler<ListMarketplacesQuery, Result<IEnumerable<Marketplace>>>
{
    private readonly IUnitOfWork unitOfWork;

    public GetAllMarketplacesQueryHandler(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<Marketplace>>> Handle(ListMarketplacesQuery query, CancellationToken cancellationToken)
    {
        var marketplaces = await unitOfWork.Repository<Marketplace>().GetAsync(
            cancellationToken: cancellationToken);

        return new Result<IEnumerable<Marketplace>>(marketplaces);
    }
}
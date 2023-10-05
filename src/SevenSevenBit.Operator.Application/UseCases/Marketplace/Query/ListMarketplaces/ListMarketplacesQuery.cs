namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.ListMarketplaces;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public class ListMarketplacesQuery : IRequest<Result<IEnumerable<Marketplace>>>
{
}
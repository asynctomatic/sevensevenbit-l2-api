namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.Query.GetMarketplace;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public record GetMarketplaceQuery(Guid MarketplaceId) : IRequest<Result<Marketplace>>;
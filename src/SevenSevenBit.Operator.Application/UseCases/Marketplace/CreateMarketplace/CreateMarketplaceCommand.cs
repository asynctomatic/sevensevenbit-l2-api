namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.CreateMarketplace;

using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public record CreateMarketplaceCommand(
    Guid BaseAssetId,
    Guid QuoteAssetId,
    string BaseAssetTokenId,
    string QuoteAssetTokenId) : IRequest<Result<Marketplace>>;
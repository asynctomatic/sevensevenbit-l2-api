namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public interface IMarketplaceService
{
    Task<Marketplace> CreateMarketplaceAsync(
        Marketplace marketplace,
        CancellationToken cancellationToken);
}
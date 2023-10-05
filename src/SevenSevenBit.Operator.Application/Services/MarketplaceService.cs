namespace SevenSevenBit.Operator.Application.Services;

using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;

public class MarketplaceService : IMarketplaceService
{
    private readonly IUnitOfWork unitOfWork;

    public MarketplaceService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<Marketplace> CreateMarketplaceAsync(Marketplace marketplace, CancellationToken cancellationToken)
    {
        await unitOfWork.Repository<Marketplace>().InsertAsync(marketplace, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        return marketplace;
    }
}
namespace SevenSevenBit.Operator.Application.UseCases.Marketplace.CreateMarketplace;

using System.ComponentModel.DataAnnotations;
using Exceptions;
using LanguageExt.Common;
using MediatR;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class CreateMarketplaceCommandHandler : IRequestHandler<CreateMarketplaceCommand, Result<Marketplace>>
{
    private readonly IAssetService assetService;
    private readonly IMarketplaceService marketplaceService;

    public CreateMarketplaceCommandHandler(IAssetService assetService, IMarketplaceService marketplaceService)
    {
        this.assetService = assetService;
        this.marketplaceService = marketplaceService;
    }

    public async Task<Result<Marketplace>> Handle(CreateMarketplaceCommand command, CancellationToken cancellationToken)
    {
        var baseAsset = await assetService.GetAssetAsync(command.BaseAssetId, cancellationToken);
        if (baseAsset is null)
        {
            // TODO: log error.
            return new Result<Marketplace>(new AssetNotFoundException(command.BaseAssetId));
        }

        var quoteAsset = await assetService.GetAssetAsync(command.QuoteAssetId, cancellationToken);
        if (quoteAsset is null)
        {
            // TODO: log error.
            return new Result<Marketplace>(new AssetNotFoundException(command.QuoteAssetId));
        }

        // TODO: move to validation pipeline.
        if ((!baseAsset.Type.IsFungible() && command.BaseAssetTokenId is null) ||
            (!quoteAsset.Type.IsFungible() && command.QuoteAssetTokenId is not null))
        {
            // TODO: log error.
        }

        var marketplace = await marketplaceService.CreateMarketplaceAsync(
            new Marketplace
            {
                BaseAssetId = baseAsset.Id,
                QuoteAssetId = quoteAsset.Id,
                BaseAssetTokenId = command.BaseAssetTokenId,
                QuoteAssetTokenId = command.QuoteAssetTokenId,
            },
            cancellationToken);

        return new Result<Marketplace>(marketplace);
    }
}
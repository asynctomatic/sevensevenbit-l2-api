namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using SevenSevenBit.Operator.Application.DTOs.Internal;
using SevenSevenBit.Operator.Domain.Entities;

public interface IMintService
{
    Task<Guid> MintAssetsAsync(
        IEnumerable<MintAssetDataDto> mints,
        CancellationToken cancellationToken);

    Task<Guid> MintAssetAsync(
        MintAssetDataDto mint,
        CancellationToken cancellationToken);
}
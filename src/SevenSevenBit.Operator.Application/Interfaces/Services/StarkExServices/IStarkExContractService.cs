namespace SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;

public interface IStarkExContractService
{
    Task<bool> IsAssetRegisteredAsync(
        string assetType);
}
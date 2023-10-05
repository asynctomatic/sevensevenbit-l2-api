namespace SevenSevenBit.Operator.Application.Blockchain;

using Nethereum.Web3;
using SevenSevenBit.Operator.Application.Blockchain.Functions;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class StarkExContractService : IStarkExContractService
{
    private readonly IWeb3 web3;

    public StarkExContractService(IWeb3 web3)
    {
        this.web3 = web3;
    }

    public async Task<bool> IsAssetRegisteredAsync(
        string assetType)
    {
        var handler = web3.Eth.GetContractQueryHandler<IsAssetRegisteredFunction>();

        var message = new IsAssetRegisteredFunction
        {
            AssetType = BigIntegerExtensions.ParseHexToPositiveBigInteger(assetType),
        };

        return await handler.QueryAsync<bool>(string.Empty, message);
        // TODO return await handler.QueryAsync<bool>(tenantContext.StarkExInstanceDetails.StarkExContractAddress, message);
    }
}
namespace SevenSevenBit.Operator.Application.Blockchain;

using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SevenSevenBit.Operator.Application.Blockchain.Functions;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.ValueObjects;

public class EthereumService : IEthereumService
{
    private const string EmptyByteCode = "0x";
    private const string MintableInterfaceId = "0x19ee6e3f";
    private const string Erc165InterfaceId = "0x01ffc9a7";

    private readonly IWeb3 web3;
    private readonly RpcRequestBuilder getCodeRpcRequestBuilder;

    public EthereumService(IWeb3 web3)
    {
        this.web3 = web3;
        getCodeRpcRequestBuilder = new RpcRequestBuilder("eth_getCode");
    }

    public async Task<bool> IsAddressASmartContractAsync(BlockchainAddress contractAddress)
    {
        var rpcRequest = getCodeRpcRequestBuilder.BuildRequest(
            Guid.NewGuid().ToString(),
            (string)contractAddress,
            BlockParameter.CreateLatest());

        var byteCode = await web3.Client.SendRequestAsync<string>(rpcRequest);

        return !byteCode.EnsureHexPrefix().Equals(EmptyByteCode);
    }

    public async Task<bool> DoesContractImplementMintableInterfaceAsync(BlockchainAddress contractAddress)
    {
        var handler = web3.Eth.GetContractQueryHandler<SupportsInterfaceFunction>();

        // Check if contract implements ERC165
        var erc165Tx = new SupportsInterfaceFunction
        {
            InterfaceId = Erc165InterfaceId.HexToByteArray(),
        };

        var implementsErc165 = await handler.QueryAsync<bool>(
            contractAddress,
            erc165Tx);

        if (!implementsErc165)
        {
            return false;
        }

        var supportsMintableInterfaceTx = new SupportsInterfaceFunction
        {
            InterfaceId = MintableInterfaceId.HexToByteArray(),
        };

        return await handler.QueryAsync<bool>(
            contractAddress,
            supportsMintableInterfaceTx);
    }
}
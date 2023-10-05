namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.Anvil;

using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.Client;

// Mines a series of blocks.
public class AnvilMine : RpcRequestResponseHandler<bool>
{
    public AnvilMine(IClient client)
        : base(client, "anvil_mine")
    {
    }

    public Task<bool> SendRequestAsync(uint blocks, object id = null)
    {
        return SendRequestAsync(id, blocks.ToString("X").EnsureHexPrefix());
    }
}
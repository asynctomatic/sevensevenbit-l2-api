namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.Anvil;

using Nethereum.JsonRpc.Client;

// Snapshot the state of the blockchain at the current block.
public class EvmSnapshot : RpcRequestResponseHandler<string>
{
    public EvmSnapshot(IClient client)
        : base(client, "evm_snapshot")
    {
    }

    public Task<string> SendRequestAsync()
    {
        return SendRequestAsync(null);
    }
}
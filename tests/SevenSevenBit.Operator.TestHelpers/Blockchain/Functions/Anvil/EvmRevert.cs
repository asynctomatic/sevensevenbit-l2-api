namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.Anvil;

using Nethereum.JsonRpc.Client;

// Revert the state of the blockchain to a previous snapshot.
public class EvmRevert : RpcRequestResponseHandler<bool>
{
    public EvmRevert(IClient client)
        : base(client, "evm_revert")
    {
    }

    public Task<bool> SendRequestAsync(string snapshot)
    {
        return SendRequestAsync(null, snapshot);
    }
}
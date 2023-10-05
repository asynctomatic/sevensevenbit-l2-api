namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.Anvil;

using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.JsonRpc.Client;

// Modifies the balance of an account.
public class AnvilSetBalance : RpcRequestResponseHandler<bool>
{
    public AnvilSetBalance(IClient client)
        : base(client, "anvil_setBalance")
    {
    }

    public Task<bool> SendRequestAsync(string account, BigInteger balance, object id = null)
    {
        return SendRequestAsync(id, account, balance.ToString("X").EnsureHexPrefix());
    }
}
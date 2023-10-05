namespace SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.StarkEx;

using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("registerEthAddress")]
public class RegisterEthAddressFunction : FunctionMessage
{
    [Parameter("address", "ethKey")]
    public string EthAddress { get; set; }

    [Parameter("uint256", "starkKey", 2)]
    public BigInteger StarkKey { get; set; }

    [Parameter("bytes", "starkSignature", 3)]
    public byte[] StarkSignature { get; set; }
}
namespace SevenSevenBit.Operator.Application.Blockchain.Functions;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

// TODO: This is never used in the app and can be moved to the test project.
[ExcludeFromCodeCoverage]
[Function("registerToken")]
public class RegisterTokenFunction : FunctionMessage
{
    [Parameter("uint256", "assetType")]
    public BigInteger AssetType { get; set; }

    [Parameter("bytes", "assetInfo", 2)]
    public byte[] AssetInfo { get; set; }

    [Parameter("uint256", "quantum", 3)]
    public BigInteger Quantum { get; set; }
}
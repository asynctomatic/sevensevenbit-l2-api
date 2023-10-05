namespace SevenSevenBit.Operator.Application.Blockchain.Functions;

using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[ExcludeFromCodeCoverage]
[Function("isAssetRegistered", "bool")]
public class IsAssetRegisteredFunction : FunctionMessage
{
    [Parameter("uint256", "assetType")]
    public BigInteger AssetType { get; set; }
}
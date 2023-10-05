namespace SevenSevenBit.Operator.Application.Blockchain.Functions;

using System.Diagnostics.CodeAnalysis;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[ExcludeFromCodeCoverage]
[Function("supportsInterface", "bool")]
public class SupportsInterfaceFunction : FunctionMessage
{
    [Parameter("bytes4", "interfaceId")]
    public byte[] InterfaceId { get; set; }
}
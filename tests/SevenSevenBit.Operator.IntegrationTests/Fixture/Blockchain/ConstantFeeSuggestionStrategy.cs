namespace SevenSevenBit.Operator.IntegrationTests.Fixture.Blockchain;

using System.Numerics;
using Nethereum.RPC.Fee1559Suggestions;

public class ConstantFeeSuggestionStrategy : IFee1559SuggestionStrategy
{
    public Task<Fee1559> SuggestFeeAsync(BigInteger? maxPriorityFeePerGas = null)
    {
        return Task.FromResult(new Fee1559
        {
            BaseFee = BigInteger.Zero,
            MaxFeePerGas = BigInteger.Zero,
            MaxPriorityFeePerGas = maxPriorityFeePerGas ?? BigInteger.Zero,
        });
    }
}
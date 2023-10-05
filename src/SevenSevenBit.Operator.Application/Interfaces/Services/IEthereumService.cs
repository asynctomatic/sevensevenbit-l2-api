namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using SevenSevenBit.Operator.Domain.ValueObjects;

public interface IEthereumService
{
    Task<bool> IsAddressASmartContractAsync(BlockchainAddress contractAddress);

    Task<bool> DoesContractImplementMintableInterfaceAsync(BlockchainAddress contractAddress);
}
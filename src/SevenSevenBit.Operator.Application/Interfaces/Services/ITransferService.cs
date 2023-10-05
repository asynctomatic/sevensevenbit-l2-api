namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;

public interface ITransferService
{
    Task<Guid> TransferAsync(
        Vault senderVault,
        Vault receiverVault,
        BigInteger amountQuantized,
        Vault feeSenderVault,
        Vault feeReceiverVault,
        BigInteger feeAmountQuantized,
        long expirationTimestamp,
        StarkSignature starkSignature,
        int nonce,
        CancellationToken cancellationToken);
}
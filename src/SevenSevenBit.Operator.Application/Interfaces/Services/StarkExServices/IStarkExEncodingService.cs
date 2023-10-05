namespace SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;

public interface IStarkExEncodingService
{
    string EncodeTransferWithFees(
        Vault senderVault,
        Vault receiverVault,
        BigInteger amountQuantized,
        BigInteger feeAmountQuantized,
        long expirationTimestamp,
        int nonce);

    string EncodeLimitOrder(
        Vault senderVault,
        VaultChainId receiverVaultId,
        StarkExAssetId receiverAssetId,
        BigInteger sellAmountQuantized,
        BigInteger buyAmountQuantized,
        long expirationTimestamp,
        int nonce,
        Vault feesVault,
        BigInteger? feeAmountQuantized);
}
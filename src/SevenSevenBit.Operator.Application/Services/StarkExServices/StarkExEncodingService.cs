namespace SevenSevenBit.Operator.Application.Services.StarkExServices;

using System.Numerics;
using Nethereum.Hex.HexConvertors.Extensions;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Crypto.SDK.Hashing;
using StarkEx.Crypto.SDK.Models;

public class StarkExEncodingService : IStarkExEncodingService
{
    private readonly ISpotTradingMessageHasher spotTradingMessageHasher;

    public StarkExEncodingService(
        ISpotTradingMessageHasher spotTradingMessageHasher)
    {
        this.spotTradingMessageHasher = spotTradingMessageHasher;
    }

    public string EncodeTransferWithFees(
        Vault senderVault,
        Vault receiverVault,
        BigInteger amountQuantized,
        BigInteger feeAmountQuantized,
        long expirationTimestamp,
        int nonce)
    {
        var transferMessage = new EncodeTransferModel
        {
            AssetIdSold = senderVault.AssetStarkExId(),
            AssetIdUsedForFees = senderVault.AssetStarkExId(), // TODO - Support using assets for fees dif from the one being transferred
            ReceiverStarkKey = receiverVault.User.StarkKey,
            VaultIdFromSender = senderVault.VaultChainId,
            VaultIdFromReceiver = receiverVault.VaultChainId,
            VaultIdUsedForFees = senderVault.VaultChainId, // TODO - Add support for paying fees from another vault (e.g. for example from a dif assetId)
            Nonce = nonce,
            QuantizedAmountToTransfer = amountQuantized.ToBouncyCastle(),
            QuantizedAmountToLimitMaxFee = feeAmountQuantized.ToBouncyCastle(),
            ExpirationTimestamp = expirationTimestamp,
        };

        var encoded = spotTradingMessageHasher.EncodeTransfer(transferMessage);

        return encoded.ToByteArray().ToHex();
    }

    public string EncodeLimitOrder(
        Vault senderVault,
        VaultChainId receiverVaultId,
        StarkExAssetId receiverAssetId,
        BigInteger sellAmountQuantized,
        BigInteger buyAmountQuantized,
        long expirationTimestamp,
        int nonce,
        Vault feesVault,
        BigInteger? feeAmountQuantized)
    {
        var transferMessage = new EncodeLimitOrderModel
        {
            AssetIdSold = senderVault.AssetStarkExId(),
            AssetIdBought = receiverAssetId,
            AssetIdUsedForFees = feesVault is not null ? feesVault.AssetStarkExId() : "0x0",
            QuantizedAmountSold = sellAmountQuantized.ToBouncyCastle(),
            QuantizedAmountBought = buyAmountQuantized.ToBouncyCastle(),
            QuantizedAmountUsedForFees = feeAmountQuantized.HasValue ? feeAmountQuantized.Value.ToBouncyCastle() : Org.BouncyCastle.Math.BigInteger.Zero,
            Nonce = nonce,
            VaultIdUsedForFees = feesVault is not null ? feesVault.VaultChainId : Org.BouncyCastle.Math.BigInteger.Zero,
            VaultIdUsedForSelling = senderVault.VaultChainId,
            VaultIdUsedForBuying = receiverVaultId,
            ExpirationTimestamp = expirationTimestamp,
        };

        var encoded = spotTradingMessageHasher.EncodeLimitOrder(transferMessage);

        return encoded.ToByteArray().ToHex();
    }
}
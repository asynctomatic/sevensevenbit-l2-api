namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using System.Numerics;
using NodaTime;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

public static class Transactions
{
    // Name should be {Operation}{User}{Asset}
    public static readonly Transaction DepositBadjoras123Erc20Usdc10000 = new()
    {
        Id = Guid.NewGuid(),
        Operation = StarkExOperation.Deposit,
        StarkExTransactionId = 0,
        Status = TransactionStatus.Pending,
        CreationDate = new LocalDateTime(2023, 2, 1, 2, 50),
        RawTransaction = new DepositModel
        {
            Amount = new BigInteger(31415926535),
            StarkKey = Users.Badjoras123.StarkKey,
            TokenId = Vaults.Badjoras123Erc20Usdc10000Validium.AssetStarkExId(),
            VaultId = Vaults.Badjoras123Erc20Usdc10000Validium.VaultChainId,
        },
    };

    public static readonly Transaction DepositBadjoras123Erc721Bayc1 = new()
    {
        Id = Guid.NewGuid(),
        Operation = StarkExOperation.Deposit,
        StarkExTransactionId = 1,
        Status = TransactionStatus.Pending,
        CreationDate = new LocalDateTime(2023, 2, 1, 3, 55),
        RawTransaction = new DepositModel
        {
            Amount = new BigInteger(1),
            StarkKey = Users.Badjoras123.StarkKey,
            TokenId = Vaults.Badjoras123Erc721Bayc1Validium.AssetStarkExId(),
            VaultId = Vaults.Badjoras123Erc721Bayc1Validium.VaultChainId,
        },
    };

    public static readonly Transaction WithdrawBadjoras123Erc20Usdc10000 = new()
    {
        Id = Guid.NewGuid(),
        Operation = StarkExOperation.Withdrawal,
        StarkExTransactionId = 2,
        Status = TransactionStatus.Pending,
        CreationDate = new LocalDateTime(2023, 2, 1, 4, 55),
        RawTransaction = new WithdrawalModel
        {
            Amount = new BigInteger(31415926535),
            StarkKey = Users.Badjoras123.StarkKey,
            TokenId = Vaults.Badjoras123Erc20Usdc10000Validium.AssetStarkExId(),
            VaultId = Vaults.Badjoras123Erc20Usdc10000Validium.VaultChainId,
        },
    };

    public static readonly Transaction MintBadjoras123Erc721Bayc1Validium = new()
    {
        Id = Guid.NewGuid(),
        Operation = StarkExOperation.Mint,
        StarkExTransactionId = 3,
        Status = TransactionStatus.Pending,
        CreationDate = new LocalDateTime(2023, 2, 1, 5, 55),
        RawTransaction = new MintModel
        {
            Amount = new BigInteger(1),
            StarkKey = Users.Badjoras123.StarkKey,
            TokenId = Vaults.Badjoras123MErc721Bayc1Validium.AssetStarkExId(),
            VaultId = Vaults.Badjoras123MErc721Bayc1Validium.VaultChainId,
        },
    };

    public static readonly Transaction TransferBadjoras1234Erc20Usdc10000 = new()
    {
        Id = Guid.NewGuid(),
        Operation = StarkExOperation.Transfer,
        StarkExTransactionId = 4,
        Status = TransactionStatus.Pending,
        CreationDate = new LocalDateTime(2023, 2, 1, 6, 55),
        RawTransaction = new TransferModel
        {
            Amount = new BigInteger(31415926535),
            SenderPublicKey = Users.Badjoras1234.StarkKey,
            ReceiverPublicKey = Users.Badjoras123.StarkKey,
            SenderVaultId = Vaults.Badjoras1234Erc20Usdc10000Validium.VaultChainId,
            ReceiverVaultId = Vaults.Badjoras123Erc20Usdc10000Validium.VaultChainId,
            Token = Assets.Erc20Usdc10000.StarkExType,
        },
    };

    public static readonly Transaction MultiTransactionWithTransferMintWithdraw = new()
    {
        Id = Guid.NewGuid(),
        Operation = StarkExOperation.MultiTransaction,
        StarkExTransactionId = 5,
        Status = TransactionStatus.Pending,
        CreationDate = new LocalDateTime(2023, 2, 1, 7, 55),
        RawTransaction = new MultiTransactionModel
        {
            Transactions = new List<TransactionModel>
            {
                new TransferModel
                {
                    Amount = new BigInteger(1000),
                    SenderPublicKey = Users.Badjoras1234.StarkKey,
                    ReceiverPublicKey = Users.Badjoras123.StarkKey,
                    SenderVaultId = Vaults.Badjoras1234Erc20Usdc10000Validium.VaultChainId,
                    ReceiverVaultId = Vaults.Badjoras123Erc20Usdc10000Validium.VaultChainId,
                    Token = Assets.Erc20Usdc10000.StarkExType,
                },
                new MintModel
                {
                    Amount = new BigInteger(1),
                    StarkKey = Users.Badjoras123.StarkKey,
                    TokenId = Vaults.Badjoras123MErc721Bayc1Validium.AssetStarkExId(),
                    VaultId = Vaults.Badjoras123MErc721Bayc1Validium.VaultChainId,
                },
                new WithdrawalModel
                {
                    Amount = new BigInteger(2000),
                    StarkKey = Users.Badjoras123.StarkKey,
                    TokenId = Vaults.Badjoras123Erc20Usdc10000Validium.AssetStarkExId(),
                    VaultId = Vaults.Badjoras123Erc20Usdc10000Validium.VaultChainId,
                },
            },
        },
    };

    public static readonly Transaction TransferBadjoras1234Erc20Usdc10000Streamed = new()
    {
        Id = Guid.NewGuid(),
        Operation = StarkExOperation.Transfer,
        StarkExTransactionId = 6,
        Status = TransactionStatus.Streamed,
        CreationDate = new LocalDateTime(2023, 2, 1, 8, 55),
        RawTransaction = new TransferModel
        {
            Amount = new BigInteger(1000),
            SenderPublicKey = Users.Badjoras1234.StarkKey,
            ReceiverPublicKey = Users.Badjoras123.StarkKey,
            ReceiverVaultId = Vaults.Badjoras1234Erc20Usdc10000Validium.VaultChainId,
            SenderVaultId = Vaults.Badjoras123Erc20Usdc10000Validium.VaultChainId,
            Token = Assets.Erc20Usdc10000.StarkExType,
        },
    };

    public static IEnumerable<Transaction> GetTransactions()
    {
        yield return DepositBadjoras123Erc20Usdc10000;
        yield return DepositBadjoras123Erc721Bayc1;
        yield return WithdrawBadjoras123Erc20Usdc10000;
        yield return MintBadjoras123Erc721Bayc1Validium;
        yield return TransferBadjoras1234Erc20Usdc10000;
        yield return MultiTransactionWithTransferMintWithdraw;
        yield return TransferBadjoras1234Erc20Usdc10000Streamed;
    }
}
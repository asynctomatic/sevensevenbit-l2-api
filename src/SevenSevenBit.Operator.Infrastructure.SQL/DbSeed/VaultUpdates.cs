namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;

public static class VaultUpdates
{
    // Name should be Vault{VaultId}Tx{TxId}Update
    public static readonly VaultUpdate Vault0Tx0Update = new()
    {
        Transaction = Transactions.DepositBadjoras123Erc20Usdc10000,
        Vault = Vaults.Badjoras123Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(31415926535),
    };

    public static readonly VaultUpdate Vault2Tx1Update = new()
    {
        Transaction = Transactions.DepositBadjoras123Erc721Bayc1,
        Vault = Vaults.Badjoras123Erc721Bayc1Validium,
        QuantizedAmount = new BigInteger(1),
    };

    public static readonly VaultUpdate Vault0Tx2Update = new()
    {
        Transaction = Transactions.WithdrawBadjoras123Erc20Usdc10000,
        Vault = Vaults.Badjoras123Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(-31415926535),
    };

    public static readonly VaultUpdate Vault3Tx3Update = new()
    {
        Transaction = Transactions.MintBadjoras123Erc721Bayc1Validium,
        Vault = Vaults.Badjoras123MErc721Bayc1Validium,
        QuantizedAmount = new BigInteger(1),
    };

    public static readonly VaultUpdate Vault0Tx4Update = new()
    {
        Transaction = Transactions.TransferBadjoras1234Erc20Usdc10000,
        Vault = Vaults.Badjoras123Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(31415926535),
    };

    public static readonly VaultUpdate Vault1Tx4Update = new()
    {
        Transaction = Transactions.TransferBadjoras1234Erc20Usdc10000,
        Vault = Vaults.Badjoras1234Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(-31415926535),
    };

    public static readonly VaultUpdate Vault0Tx5Update = new()
    {
        Transaction = Transactions.MultiTransactionWithTransferMintWithdraw,
        Vault = Vaults.Badjoras123Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(1000),
    };

    public static readonly VaultUpdate Vault1Tx5Update = new()
    {
        Transaction = Transactions.MultiTransactionWithTransferMintWithdraw,
        Vault = Vaults.Badjoras1234Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(-1000),
    };

    public static readonly VaultUpdate Vault3Tx5Update = new()
    {
        Transaction = Transactions.MultiTransactionWithTransferMintWithdraw,
        Vault = Vaults.Badjoras123MErc721Bayc1Validium,
        QuantizedAmount = new BigInteger(1),
    };

    public static readonly VaultUpdate Vault3WithdrawTx5Update = new()
    {
        Transaction = Transactions.MultiTransactionWithTransferMintWithdraw,
        Vault = Vaults.Badjoras123Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(-2000),
    };

    public static readonly VaultUpdate Vault0TransferTx6Update = new()
    {
        Transaction = Transactions.TransferBadjoras1234Erc20Usdc10000Streamed,
        Vault = Vaults.Badjoras123Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(-1000),
    };

    public static readonly VaultUpdate Vault1TransferTx6Update = new()
    {
        Transaction = Transactions.TransferBadjoras1234Erc20Usdc10000Streamed,
        Vault = Vaults.Badjoras1234Erc20Usdc10000Validium,
        QuantizedAmount = new BigInteger(1000),
    };

    public static IEnumerable<VaultUpdate> GetVaultUpdates()
    {
        yield return Vault0Tx0Update;
        yield return Vault2Tx1Update;
        yield return Vault0Tx2Update;
        yield return Vault3Tx3Update;
        yield return Vault0Tx4Update;
        yield return Vault1Tx4Update;
        yield return Vault0Tx5Update;
        yield return Vault1Tx5Update;
        yield return Vault3Tx5Update;
        yield return Vault3WithdrawTx5Update;
        yield return Vault0TransferTx6Update;
        yield return Vault1TransferTx6Update;
    }
}
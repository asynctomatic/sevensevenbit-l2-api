namespace SevenSevenBit.Operator.Infrastructure.SQL.DbSeed;

using System.Numerics;
using SevenSevenBit.Operator.Domain.Entities;

public static class ReplacementVaultUpdates
{
    // Name should be Vault{VaultId}ReplacementTx{TxId}Update
    public static readonly ReplacementVaultUpdate Vault2ReplacementTx1Update = new()
    {
        ReplacementTransaction = ReplacementTransactions.ReplacedDepositBadjoras123Erc721Bayc1AtMaxReplacements,
        Vault = Vaults.Badjoras123Erc721Bayc1Validium,
        QuantizedAmount = BigInteger.One,
    };

    public static IEnumerable<ReplacementVaultUpdate> GetReplacementVaultUpdates()
    {
        yield return Vault2ReplacementTx1Update;
    }
}
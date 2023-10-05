namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Common;

[Table("replacement_vault_updates")]
public class ReplacementVaultUpdate : BaseEntity
{
    private Vault vault;
    private ReplacementTransaction replacementTransaction;

    public ReplacementVaultUpdate()
    {
    }

    private ReplacementVaultUpdate(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("vault_id")]
    public Guid VaultId { get; set; }

    [Column("replacement_tx_id")]
    public Guid ReplacementTransactionId { get; set; }

    [Column("amount", TypeName = "numeric")]
    public BigInteger QuantizedAmount { get; set; }

    public Vault Vault
    {
        get => LazyLoader.Load(this, ref vault);
        set => vault = value;
    }

    public ReplacementTransaction ReplacementTransaction
    {
        get => LazyLoader.Load(this, ref replacementTransaction);
        set => replacementTransaction = value;
    }
}
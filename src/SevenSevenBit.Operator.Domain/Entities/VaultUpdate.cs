namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Common;

[Table("vault_updates")]
public class VaultUpdate : BaseEntity
{
    private Vault vault;
    private Transaction transaction;

    public VaultUpdate()
    {
    }

    private VaultUpdate(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("vault_id")]
    public Guid VaultId { get; set; }

    [Column("tx_id")]
    public Guid TransactionId { get; set; }

    [Column("amount", TypeName = "numeric")]
    public BigInteger QuantizedAmount { get; set; }

    public Vault Vault
    {
        get => LazyLoader.Load(this, ref vault);
        set => vault = value;
    }

    public Transaction Transaction
    {
        get => LazyLoader.Load(this, ref transaction);
        set => transaction = value;
    }
}
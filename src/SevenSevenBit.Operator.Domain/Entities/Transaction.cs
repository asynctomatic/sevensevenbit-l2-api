namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NodaTime;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

[Table("transactions")]
public class Transaction : BaseEntity
{
    private List<VaultUpdate> vaultUpdates = new();
    private List<ReplacementTransaction> replacementTransactions = new();

    public Transaction()
    {
    }

    private Transaction(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("starkex_tx_id")]
    public long StarkExTransactionId { get; set; }

    [Required]
    [Column(name: "raw_transaction", TypeName = "jsonb")]
    public TransactionModel RawTransaction { get; set; }

    [Column("operation")]
    public StarkExOperation Operation { get; set; }

    [Column("status")]
    public TransactionStatus Status { get; set; }

    [Column("creation_date")]
    public LocalDateTime CreationDate { get; set; }

    public List<VaultUpdate> VaultUpdates
    {
        get => LazyLoader.Load(this, ref vaultUpdates);
        set => vaultUpdates = value;
    }

    public List<ReplacementTransaction> ReplacementTransactions
    {
        get => LazyLoader.Load(this, ref replacementTransactions);
        set => replacementTransactions = value;
    }
}
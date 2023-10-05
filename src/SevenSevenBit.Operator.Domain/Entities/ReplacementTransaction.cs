namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NodaTime;
using SevenSevenBit.Operator.Domain.Common;
using StarkEx.Client.SDK.Enums.Spot;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

[Table("replacement_transactions")]
public class ReplacementTransaction : BaseEntity
{
    private Transaction transaction;
    private List<ReplacementVaultUpdate> replacementVaultUpdates;

    public ReplacementTransaction()
    {
    }

    private ReplacementTransaction(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("transaction_id")]
    public Guid TransactionId { get; set; }

    [Required]
    [Column("error_code")]
    public SpotApiCodes ErrorCode { get; set; }

    [Required]
    [Column("error_msg")]
    public string ErrorMessage { get; set; }

    [Required]
    [Column("reverted_date")]
    public LocalDateTime RevertedDate { get; set; }

    [Column("replacement_counter")]
    public int ReplacementCounter { get; set; }

    [Required]
    [Column(name: "raw_replacement_transactions", TypeName = "jsonb")]
    public IEnumerable<TransactionModel> RawReplacementTransactions { get; set; }

    public Transaction Transaction
    {
        get => LazyLoader.Load(this, ref transaction);
        set => transaction = value;
    }

    public List<ReplacementVaultUpdate> ReplacementVaultUpdates
    {
        get => LazyLoader.Load(this, ref replacementVaultUpdates);
        set => replacementVaultUpdates = value;
    }
}
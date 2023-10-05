namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Enums;

[Table("fees_config")]
public class FeeConfig : BaseEntity
{
    public FeeConfig()
    {
    }

    private FeeConfig(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Column("action")]
    public FeeAction Action { get; set; }

    [Column("amount")]
    public int Amount { get; set; }
}
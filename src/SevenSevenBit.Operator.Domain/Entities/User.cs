namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NodaTime;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.ValueObjects;

[Table("users")]
public class User : BaseEntity
{
    private List<Vault> vaults = new();
    private List<MarketplaceOrder> orders = new();

    public User()
    {
    }

    private User(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("stark_key")]
    public StarkKey StarkKey { get; set; }

    [Column("creation_date")]
    public LocalDateTime CreationDate { get; set; }

    public uint Version { get; set; }

    public List<Vault> Vaults
    {
        get => LazyLoader.Load(this, ref vaults);
        set => vaults = value;
    }

    public List<MarketplaceOrder> Orders
    {
        get => LazyLoader.Load(this, ref orders);
        set => orders = value;
    }
}
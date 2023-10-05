namespace SevenSevenBit.Operator.Domain.Entities.Marketplace;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NodaTime;
using SevenSevenBit.Operator.Domain.Common;

[Table("order_matches")]
public class OrderMatch : BaseEntity
{
    // private MarketplaceOrder makerOrder;
    // private MarketplaceOrder takerOrder;
    private Transaction transaction;

    public OrderMatch()
    {
    }

    private OrderMatch(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("maker_order_id")]
    public Guid MakerOrderId { get; set; }

    [Required]
    [Column("taker_order_id")]
    public Guid TakerOrderId { get; set; }

    [Required]
    [Column("quantity")]
    public BigInteger Quantity { get; set; }

    [Required]
    [Column("price")]
    public BigInteger Price { get; set; }

    [Required]
    [Column("created_at")]
    public LocalDateTime CreatedAt { get; set; }

    [Required]
    [Column("transaction_id")]
    public Guid TransactionId { get; set; }

    /*public MarketplaceOrder MakerOrder
    {
        get => LazyLoader.Load(this, ref makerOrder);
        set => makerOrder = value;
    }*/

    /*public MarketplaceOrder TakerOrder
    {
        get => LazyLoader.Load(this, ref takerOrder);
        set => takerOrder = value;
    }*/

    public Transaction Transaction
    {
        get => LazyLoader.Load(this, ref transaction);
        set => transaction = value;
    }
}
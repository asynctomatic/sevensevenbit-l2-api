namespace SevenSevenBit.Operator.Domain.Entities.Marketplace;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Common;

[Table("marketplaces")]
public class Marketplace : BaseEntity
{
    private Asset baseAsset;
    private Asset quoteAsset;
    private List<MarketplaceOrder> orders;

    public Marketplace()
    {
    }

    private Marketplace(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("base_asset_id")]
    public Guid BaseAssetId { get; set; }

    [Required]
    [Column("quote_asset_id")]
    public Guid QuoteAssetId { get; set; }

    [Column("base_asset_token_id")]
    public string BaseAssetTokenId { get; set; }

    [Column("quote_asset_token_id")]
    public string QuoteAssetTokenId { get; set; }

    public Asset BaseAsset
    {
        get => LazyLoader.Load(this, ref baseAsset);
        set => baseAsset = value;
    }

    public Asset QuoteAsset
    {
        get => LazyLoader.Load(this, ref quoteAsset);
        set => quoteAsset = value;
    }

    public List<MarketplaceOrder> Orders
    {
        get => LazyLoader.Load(this, ref orders);
        set => orders = value;
    }
}
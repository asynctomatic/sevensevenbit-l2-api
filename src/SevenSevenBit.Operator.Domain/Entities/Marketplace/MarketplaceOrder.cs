namespace SevenSevenBit.Operator.Domain.Entities.Marketplace;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NodaTime;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Enums;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

[Table("marketplace_orders")]
public class MarketplaceOrder : BaseEntity
{
    private Marketplace marketplace;
    private User user;
    private Vault baseAssetVault;
    private Vault quoteAssetVault;
    private List<OrderMatch> matches;

    public MarketplaceOrder()
    {
    }

    private MarketplaceOrder(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("marketplace_id")]
    public Guid MarketplaceId { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("base_asset_vault_id")]
    public Guid BaseAssetVaultId { get; set; }

    [Required]
    [Column("quote_asset_vault_id")]
    public Guid QuoteAssetVaultId { get; set; }

    [Required]
    [Column("type")]
    public OrderType Type { get; set; }

    [Required]
    [Column("side")]
    public OrderSide Side { get; set; }

    [Required]
    [Column("status")]
    public OrderStatus Status { get; set; }

    [Required]
    [Column("created_at")]
    public LocalDateTime CreatedAt { get; set; }

    [Required]
    [Column(name: "raw_order_model", TypeName = "jsonb")]
    public OrderRequestModel OrderModel { get; set; }

    public Marketplace Marketplace
    {
        get => LazyLoader.Load(this, ref marketplace);
        set => marketplace = value;
    }

    public User User
    {
        get => LazyLoader.Load(this, ref user);
        set => user = value;
    }

    public Vault BaseAssetVault
    {
        get => LazyLoader.Load(this, ref baseAssetVault);
        set => baseAssetVault = value;
    }

    public Vault QuoteAssetVault
    {
        get => LazyLoader.Load(this, ref quoteAssetVault);
        set => quoteAssetVault = value;
    }

    public List<OrderMatch> Matches
    {
        get => LazyLoader.Load(this, ref matches);
        set => matches = value;
    }

    public BigInteger Size =>
        Side == OrderSide.Bid
            ? OrderModel.SellAmount
            : OrderModel.BuyAmount;

    public BigInteger AvailableSize =>
        Size - Matches.Aggregate(BigInteger.Zero, (acc, match) => acc + match.Quantity);

    public BigInteger Price =>
        Side == OrderSide.Bid
            ? OrderModel.SellAmount / OrderModel.BuyAmount
            : OrderModel.BuyAmount / OrderModel.SellAmount;

    public bool IsActive() => Status is OrderStatus.Placed or OrderStatus.PartiallyFilled;

    public void RollbackMatch(OrderMatch match)
    {
        Status = OrderStatus.Cancelled; // TODO: CHANGE TO REVERTED
        // TODO
    }

    public Option<OrderMatch> MatchOrder(MarketplaceOrder takerOrder)
    {
        // Calculate fill amount.
        var fillAmount = BigInteger.Min(Size, takerOrder.AvailableSize);
        if (fillAmount == BigInteger.Zero)
        {
            return default;
        }

        // Calculate fill price.
        var fillPrice = Side == OrderSide.Bid
            ? takerOrder.Price
            : Price;

        // Create match.
        var match = new OrderMatch
        {
            MakerOrderId = Id,
            TakerOrderId = takerOrder.Id,
            Quantity = fillAmount,
            Price = fillPrice,
            Transaction = new Transaction
            {
                Status = TransactionStatus.Streamed,
                Operation = StarkExOperation.Settlement,
                RawTransaction = new SettlementModel
                {
                    PartyA = OrderModel,
                    PartyB = takerOrder.OrderModel,
                    SettlementInfo = new SettlementInfoModel
                    {
                        PartyASold = BigInteger.Zero,   // TODO: calculate
                        PartyBSold = BigInteger.Zero,   // TODO: calculate
                        PartyAInfo = new FeeInfoExchangeModel(),    // TODO: add fee model
                        PartyBInfo = new FeeInfoExchangeModel(),    // TODO: add fee model
                    },
                },
            },
        };

        // Update maker order.
        Matches.Add(match);
        Status = AvailableSize == BigInteger.Zero ?
            OrderStatus.Filled : OrderStatus.PartiallyFilled;

        // Update taker order.
        takerOrder.Matches.Add(match);
        takerOrder.Status = takerOrder.AvailableSize == BigInteger.Zero ?
            OrderStatus.Filled : OrderStatus.PartiallyFilled;

        return match;
    }
}
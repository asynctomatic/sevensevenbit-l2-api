namespace SevenSevenBit.Operator.Domain.Common;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.ValueObjects;

public abstract class BaseMintingBlob : BaseEntity
{
    private Asset asset;
    private List<Vault> vaults = new();

    public BaseMintingBlob()
    {
    }

    protected BaseMintingBlob(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("asset_id")]
    public Guid AssetId { get; set; }

    [Column("product_id")]
    public Guid? ProductId { get; set; }

    [Column("vault_id")]
    public Guid? VaultId { get; set; }

    [Required]
    [Column("minting_blob")]
    public MintingBlob MintingBlobHex { get; set; }

    [Required]
    [Column("quantity", TypeName = "numeric")]
    public BigInteger QuantizedQuantity { get; set; }

    public Asset Asset
    {
        get => LazyLoader.Load(this, ref asset);
        set => asset = value;
    }

    public List<Vault> Vaults
    {
        get => LazyLoader.Load(this, ref vaults);
        set => vaults = value;
    }
}
namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.ValueObjects;
using StarkEx.Crypto.SDK.Enums;

[Table("assets")]
public class Asset : BaseEntity
{
    private IEnumerable<Vault> vaults;
    private IEnumerable<BaseMintingBlob> mintingBlobs;

    public Asset()
    {
    }

    private Asset(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("starkex_type")]
    public StarkExAssetType StarkExType { get; set; }

    [Column("type")]
    public AssetType Type { get; set; }

    [Column("address")]
    public BlockchainAddress Address { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; }

    [Required]
    [Column("symbol")]
    public string Symbol { get; set; }

    [Required]
    [Column("quantum", TypeName = "numeric")]
    public Quantum Quantum { get; set; }

    [Column("enabled")]
    public bool Enabled { get; set; }

    public IEnumerable<Vault> Vaults
    {
        get => LazyLoader.Load(this, ref vaults);
        set => vaults = value;
    }

    public IEnumerable<BaseMintingBlob> MintingBlobs
    {
        get => LazyLoader.Load(this, ref mintingBlobs);
        set => mintingBlobs = value;
    }
}
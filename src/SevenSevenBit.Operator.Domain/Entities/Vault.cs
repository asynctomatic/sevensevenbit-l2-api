namespace SevenSevenBit.Operator.Domain.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.SharedKernel.Extensions;
using StarkEx.Crypto.SDK.Encoding;

[Table("vaults")]
public class Vault : BaseEntity
{
    private Asset asset;
    private User user;
    private List<VaultUpdate> vaultUpdates;
    private List<ReplacementVaultUpdate> replacementVaultUpdates;
    private BaseMintingBlob? baseMintingBlob;

    public Vault()
    {
    }

    private Vault(ILazyLoader lazyLoader)
        : base(lazyLoader)
    {
    }

    [Key]
    [Column("id")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Required]
    [Column("asset_id")]
    public Guid AssetId { get; set; }

    [Column("minting_blob_id")]
    public Guid? MintingBlobId { get; set; }

    [Column("vault_chain_id", TypeName = "numeric")]
    public VaultChainId VaultChainId { get; set; }

    [Column("product_id")]
    public Guid? ProductId { get; set; }

    [Column("token_id")]
    public TokenId TokenId { get; set; }

    [Column("da_mode")]
    public DataAvailabilityModes DataAvailabilityMode { get; set; } = DataAvailabilityModes.Validium;

    [Column("available_balance", TypeName = "numeric")]
    public BigInteger QuantizedAvailableBalance { get; set; }

    [Column("accounting_balance", TypeName = "numeric")]
    public BigInteger QuantizedAccountingBalance { get; set; }

    public uint Version { get; set; }

    public Asset Asset
    {
        get => LazyLoader.Load(this, ref asset);
        set => asset = value;
    }

    public User User
    {
        get => LazyLoader.Load(this, ref user);
        set => user = value;
    }

    public List<VaultUpdate> VaultUpdates
    {
        get => LazyLoader.Load(this, ref vaultUpdates);
        set => vaultUpdates = value;
    }

    public List<ReplacementVaultUpdate> ReplacementVaultUpdates
    {
        get => LazyLoader.Load(this, ref replacementVaultUpdates);
        set => replacementVaultUpdates = value;
    }

    public virtual BaseMintingBlob? BaseMintingBlob
    {
        get => LazyLoader.Load(this, ref baseMintingBlob);
        set => baseMintingBlob = value;
    }

    public StarkExAssetId AssetStarkExId(bool isMintableOperation = false) => GetAssetStarkExId(isMintableOperation);

    private StarkExAssetId GetAssetStarkExId(bool isMintableOperation)
    {
        if ((Asset.Type.HasTokenId() && TokenId is null) || (Asset.Type.IsMintable() && BaseMintingBlob is null))
        {
            return null;
        }

        return AssetEncoder.GetAssetId(
                    Asset.Type,
                    address: Asset.Address,
                    quantum: Asset.Quantum,
                    tokenId: TokenId,
                    mintingBlob: BaseMintingBlob?.MintingBlobHex,
                    isMintingTransaction: isMintableOperation);
    }
}
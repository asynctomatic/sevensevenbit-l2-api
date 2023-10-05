namespace SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;

using MassTransit;
using Microsoft.EntityFrameworkCore;
using SevenSevenBit.Operator.Domain.Common;
using SevenSevenBit.Operator.Domain.Constants;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Entities.Marketplace;
using SevenSevenBit.Operator.Domain.Entities.MintingBlobs;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;
using SevenSevenBit.Operator.Infrastructure.SQL.ValueConverters;
using SevenSevenBit.Operator.SharedKernel.Extensions;

public class OperatorDbContext : DbContext
{
    public OperatorDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // MassTransit Entities
        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();

        // User Entity
        // This is necessary to prevent using duplicated nonces.
        // https://www.npgsql.org/efcore/modeling/concurrency.html?tabs=fluent-api
        modelBuilder.Entity<User>()
            .Property(u => u.Version)
            .IsRowVersion();

        modelBuilder
            .Entity<User>()
            .Property(u => u.CreationDate)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.StarkKey)
            .IsUnique();

        modelBuilder
            .Entity<User>()
            .HasMany(u => u.Vaults)
            .WithOne(v => v.User)
            .HasForeignKey(v => v.UserId)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Asset Entity
        modelBuilder.Entity<Asset>()
            .HasIndex(a => a.StarkExType)
            .IsUnique();

        modelBuilder.Entity<Asset>()
            .Property(x => x.Type)
            .HasConversion<string>();

        modelBuilder
            .Entity<Asset>()
            .HasMany(a => a.Vaults)
            .WithOne(v => v.Asset)
            .HasForeignKey(v => v.AssetId)
            .HasPrincipalKey(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder
            .Entity<Asset>()
            .HasMany(a => a.MintingBlobs)
            .WithOne(v => v.Asset)
            .HasForeignKey(v => v.AssetId)
            .HasPrincipalKey(a => a.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Minting Blobs Entities
        modelBuilder.Entity<BaseMintingBlob>().UseTpcMappingStrategy();

        modelBuilder.Entity<BaseMintingBlob>()
            .HasIndex(u => new { u.AssetId, u.MintingBlobHex })
            .IsUnique();

        modelBuilder
            .Entity<BaseMintingBlob>()
            .HasOne(blob => blob.Asset)
            .WithMany(a => a.MintingBlobs)
            .HasForeignKey(blob => blob.AssetId)
            .HasPrincipalKey(asset => asset.Id);

        // TODO update the OperatorDbContext adding the stuff of the Marketplace
        modelBuilder
            .Entity<BaseMintingBlob>()
            .HasMany(blob => blob.Vaults)
            .WithOne(vault => vault.BaseMintingBlob)
            .HasForeignKey(v => v.MintingBlobId)
            .IsRequired(false);

        modelBuilder.Entity<Erc20MintingBlob>();
        modelBuilder.Entity<Erc721MintingBlob>();
        modelBuilder.Entity<Erc1155MintingBlob>();

        // FeeConfig Entity
        modelBuilder.Entity<FeeConfig>()
            .Property(fC => fC.Action)
            .HasConversion<string>();

        modelBuilder.Entity<FeeConfig>()
            .HasIndex(u => u.Action)
            .IsUnique();

        // Vault Entity
        // This is necessary to prevent using duplicated nonces.
        // https://www.npgsql.org/efcore/modeling/concurrency.html?tabs=fluent-api
        modelBuilder.Entity<Vault>()
            .Property(v => v.Version)
            .IsRowVersion();

        modelBuilder.Entity<Vault>()
            .HasIndex(v => v.VaultChainId)
            .IsUnique();

        modelBuilder.Entity<Vault>()
            .Property(v => v.DataAvailabilityMode)
            .HasConversion<string>();

        modelBuilder.Entity<Vault>()
            .HasOne(v => v.Asset)
            .WithMany(a => a.Vaults)
            .HasForeignKey(v => v.AssetId)
            .HasPrincipalKey(a => a.Id);

        modelBuilder.Entity<Vault>()
            .HasOne(v => v.User)
            .WithMany(a => a.Vaults)
            .HasForeignKey(v => v.UserId)
            .HasPrincipalKey(u => u.Id);

        modelBuilder.Entity<Vault>()
            .HasMany(vu => vu.VaultUpdates)
            .WithOne(v => v.Vault)
            .HasForeignKey(vu => vu.VaultId)
            .HasPrincipalKey(v => v.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Vault>()
            .HasMany(vu => vu.ReplacementVaultUpdates)
            .WithOne(v => v.Vault)
            .HasForeignKey(vu => vu.VaultId)
            .HasPrincipalKey(v => v.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.HasSequence<long>("VaultChainId_ValidiumSequence")
            .StartsAt((long)StarkExConstants.ValidiumVaultsLowerBound + 1) // The first vault chain id is 1.
            .HasMax((long)StarkExConstants.ValidiumVaultsUpperBound)
            .IncrementsBy(1);
        modelBuilder.Entity<Vault>()
            .Property(e => e.VaultChainId)
            .HasDefaultValueSql("nextval('\"VaultChainId_ValidiumSequence\"')");

        // VaultUpdate Entity
        modelBuilder.Entity<VaultUpdate>()
            .HasOne(vu => vu.Vault)
            .WithMany(v => v.VaultUpdates)
            .HasForeignKey(vu => vu.VaultId)
            .HasPrincipalKey(v => v.Id);

        modelBuilder.Entity<VaultUpdate>()
            .HasOne(vu => vu.Transaction)
            .WithMany(t => t.VaultUpdates)
            .HasForeignKey(vu => vu.TransactionId)
            .HasPrincipalKey(v => v.Id);

        // Transaction Entity
        modelBuilder.Entity<Transaction>()
            .Property(x => x.Status)
            .HasConversion<string>()
            .HasDefaultValue(TransactionStatus.Created);

        modelBuilder
            .Entity<Transaction>()
            .Property(e => e.CreationDate)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<Transaction>()
            .Property(x => x.Operation)
            .HasConversion<string>();

        modelBuilder.Entity<Transaction>()
            .HasIndex(u => u.StarkExTransactionId)
            .IsUnique();

        modelBuilder.Entity<Transaction>()
            .HasMany(t => t.VaultUpdates)
            .WithOne(vu => vu.Transaction)
            .HasForeignKey(vu => vu.TransactionId)
            .HasPrincipalKey(v => v.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Transaction>()
            .HasMany(t => t.ReplacementTransactions)
            .WithOne(vu => vu.Transaction)
            .HasForeignKey(vu => vu.TransactionId)
            .HasPrincipalKey(v => v.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Replacement Transaction Entity
        modelBuilder.Entity<ReplacementTransaction>()
            .Property(x => x.ErrorCode)
            .HasConversion<string>();

        modelBuilder
            .Entity<ReplacementTransaction>()
            .Property(e => e.RevertedDate)
            .HasDefaultValueSql("now()");

        modelBuilder.Entity<ReplacementTransaction>()
            .HasOne(t => t.Transaction)
            .WithMany(vu => vu.ReplacementTransactions)
            .HasForeignKey(vu => vu.TransactionId)
            .HasPrincipalKey(v => v.Id);

        modelBuilder.Entity<ReplacementTransaction>()
            .HasMany(t => t.ReplacementVaultUpdates)
            .WithOne(vu => vu.ReplacementTransaction)
            .HasForeignKey(vu => vu.ReplacementTransactionId)
            .HasPrincipalKey(v => v.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Replacement Vault Updates Entity
        modelBuilder.Entity<ReplacementVaultUpdate>()
            .HasOne(vu => vu.Vault)
            .WithMany(v => v.ReplacementVaultUpdates)
            .HasForeignKey(vu => vu.VaultId)
            .HasPrincipalKey(v => v.Id);

        modelBuilder.Entity<ReplacementVaultUpdate>()
            .HasOne(vu => vu.ReplacementTransaction)
            .WithMany(v => v.ReplacementVaultUpdates)
            .HasForeignKey(vu => vu.ReplacementTransactionId)
            .HasPrincipalKey(v => v.Id);

        // Marketplace
        ConfigureMarketplaceModel(modelBuilder);
        ConfigureMarketplaceOrderModel(modelBuilder);
        ConfigureOrderMatchModel(modelBuilder);
    }

    private void ConfigureMarketplaceModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Marketplace>()
            .HasOne(marketplace => marketplace.BaseAsset)
            .WithMany() // Single-directional relationship.
            .HasForeignKey(marketplace => marketplace.BaseAssetId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Marketplace>()
            .HasOne(marketplace => marketplace.QuoteAsset)
            .WithMany() // Single-directional relationship.
            .HasForeignKey(marketplace => marketplace.QuoteAssetId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private void ConfigureMarketplaceOrderModel(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MarketplaceOrder>()
            .HasOne(order => order.Marketplace)
            .WithMany(marketplace => marketplace.Orders)
            .HasForeignKey(order => order.MarketplaceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MarketplaceOrder>()
            .HasOne(order => order.User)
            .WithMany() // Single-directional relationship.
            .HasForeignKey(order => order.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MarketplaceOrder>()
            .Property(x => x.Type)
            .HasConversion(
                enumVersion => enumVersion.ToEnumString(),
                stringVersion => stringVersion.ToEnum<OrderType>());

        modelBuilder.Entity<MarketplaceOrder>()
            .Property(x => x.Side)
            .HasConversion(
                enumVersion => enumVersion.ToEnumString(),
                stringVersion => stringVersion.ToEnum<OrderSide>());

        modelBuilder.Entity<MarketplaceOrder>()
            .Property(x => x.Status)
            .HasConversion(
                enumVersion => enumVersion.ToEnumString(),
                stringVersion => stringVersion.ToEnum<OrderStatus>());

        modelBuilder.Entity<MarketplaceOrder>()
            .Property(order => order.CreatedAt)
            .HasDefaultValueSql("now()");

        //modelBuilder.Entity<OrderMatch>()
        //    .HasOne(p => p.MakerOrder)
        //    .WithMany(b => b.Matches)
        //    .HasForeignKey(p => p.MakerOrderId)
        //    .HasConstraintName("FK_OrderMatch_MakerOrder");

        //modelBuilder.Entity<OrderMatch>()
        //    .HasOne(p => p.TakerOrder)
        //    .WithMany() // No navigation property for this relationship
        //    .HasForeignKey(p => p.TakerOrderId)
        //    .HasConstraintName("FK_OrderMatch_TakerOrder")
        //    .OnDelete(DeleteBehavior.Restrict); // To avoid multiple cascade paths
    }

    private void ConfigureOrderMatchModel(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<OrderMatch>()
            .HasOne(match => match.MakerOrder)
            .WithMany(order => order.Matches)
            .HasForeignKey(match => match.MakerOrderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderMatch>()
            .HasOne(match => match.TakerOrder)
            .WithMany(order => order.Matches)
            .HasForeignKey(match => match.TakerOrderId)
            .OnDelete(DeleteBehavior.Restrict);*/

        modelBuilder.Entity<OrderMatch>()
            .HasOne(match => match.Transaction)
            .WithMany() // Single-directional relationship.
            .HasForeignKey(order => order.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderMatch>()
            .Property(order => order.CreatedAt)
            .HasDefaultValueSql("now()");
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<BlockchainAddress>()
            .HaveConversion<BlockchainAddressConverter>();

        configurationBuilder
            .Properties<MintingBlob>()
            .HaveConversion<MintingBlobConverter>();

        configurationBuilder
            .Properties<TokenId>()
            .HaveConversion<TokenIdConverter>();

        configurationBuilder
            .Properties<StarkExAssetType>()
            .HaveConversion<StarkExAssetTypeConverter>();

        configurationBuilder
            .Properties<StarkKey>()
            .HaveConversion<StarkKeyConverter>();

        configurationBuilder
            .Properties<Quantum>()
            .HaveConversion<QuantumConverter>();

        configurationBuilder
            .Properties<VaultChainId>()
            .HaveConversion<VaultChainIdConverter>();
    }
}
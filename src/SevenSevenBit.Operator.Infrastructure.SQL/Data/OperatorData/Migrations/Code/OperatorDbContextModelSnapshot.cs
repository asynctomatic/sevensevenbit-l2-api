﻿// <auto-generated />
using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

#nullable disable

namespace SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData.Migrations.Code
{
    [DbContext(typeof(OperatorDbContext))]
    partial class OperatorDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.HasSequence("VaultChainId_ValidiumSequence")
                .HasMax(2147483648L);

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.InboxState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("Consumed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid");

                    b.Property<int>("ReceiveCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Received")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasAlternateKey("MessageId", "ConsumerId");

                    b.HasIndex("Delivered");

                    b.ToTable("InboxState");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.Property<long>("SequenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SequenceNumber"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("EnqueueTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FaultAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Headers")
                        .HasColumnType("text");

                    b.Property<Guid?>("InboxConsumerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("InboxMessageId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("InitiatorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("OutboxId")
                        .HasColumnType("uuid");

                    b.Property<string>("Properties")
                        .HasColumnType("text");

                    b.Property<Guid?>("RequestId")
                        .HasColumnType("uuid");

                    b.Property<string>("ResponseAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("SequenceNumber");

                    b.HasIndex("EnqueueTime");

                    b.HasIndex("ExpirationTime");

                    b.HasIndex("OutboxId", "SequenceNumber")
                        .IsUnique();

                    b.HasIndex("InboxMessageId", "InboxConsumerId", "SequenceNumber")
                        .IsUnique();

                    b.ToTable("OutboxMessage");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxState", b =>
                {
                    b.Property<Guid>("OutboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.HasKey("OutboxId");

                    b.HasIndex("Created");

                    b.ToTable("OutboxState");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Common.BaseMintingBlob", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AssetId")
                        .HasColumnType("uuid")
                        .HasColumnName("asset_id");

                    b.Property<string>("MintingBlobHex")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("minting_blob");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<BigInteger>("QuantizedQuantity")
                        .HasColumnType("numeric")
                        .HasColumnName("quantity");

                    b.Property<Guid?>("VaultId")
                        .HasColumnType("uuid")
                        .HasColumnName("vault_id");

                    b.HasKey("Id");

                    b.HasIndex("AssetId", "MintingBlobHex")
                        .IsUnique();

                    b.ToTable((string)null);

                    b.UseTpcMappingStrategy();
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Asset", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<bool>("Enabled")
                        .HasColumnType("boolean")
                        .HasColumnName("enabled");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<BigInteger>("Quantum")
                        .HasColumnType("numeric")
                        .HasColumnName("quantum");

                    b.Property<string>("StarkExType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("starkex_type");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("symbol");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.HasKey("Id");

                    b.HasIndex("StarkExType")
                        .IsUnique();

                    b.ToTable("assets");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.FeeConfig", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Action")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("action");

                    b.Property<int>("Amount")
                        .HasColumnType("integer")
                        .HasColumnName("amount");

                    b.HasKey("Id");

                    b.HasIndex("Action")
                        .IsUnique();

                    b.ToTable("fees_config");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.Marketplace", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BaseAssetId")
                        .HasColumnType("uuid")
                        .HasColumnName("base_asset_id");

                    b.Property<string>("BaseAssetTokenId")
                        .HasColumnType("text")
                        .HasColumnName("base_asset_token_id");

                    b.Property<Guid>("QuoteAssetId")
                        .HasColumnType("uuid")
                        .HasColumnName("quote_asset_id");

                    b.Property<string>("QuoteAssetTokenId")
                        .HasColumnType("text")
                        .HasColumnName("quote_asset_token_id");

                    b.HasKey("Id");

                    b.HasIndex("BaseAssetId");

                    b.HasIndex("QuoteAssetId");

                    b.ToTable("marketplaces");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.MarketplaceOrder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("BaseAssetVaultId")
                        .HasColumnType("uuid")
                        .HasColumnName("base_asset_vault_id");

                    b.Property<LocalDateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("MarketplaceId")
                        .HasColumnType("uuid")
                        .HasColumnName("marketplace_id");

                    b.Property<OrderRequestModel>("OrderModel")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("raw_order_model");

                    b.Property<Guid>("QuoteAssetVaultId")
                        .HasColumnType("uuid")
                        .HasColumnName("quote_asset_vault_id");

                    b.Property<string>("Side")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("side");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("type");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<Guid?>("UserId1")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("BaseAssetVaultId");

                    b.HasIndex("MarketplaceId");

                    b.HasIndex("QuoteAssetVaultId");

                    b.HasIndex("UserId");

                    b.HasIndex("UserId1");

                    b.ToTable("marketplace_orders");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.OrderMatch", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<LocalDateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("MakerOrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("maker_order_id");

                    b.Property<Guid?>("MarketplaceOrderId")
                        .HasColumnType("uuid");

                    b.Property<BigInteger>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<BigInteger>("Quantity")
                        .HasColumnType("numeric")
                        .HasColumnName("quantity");

                    b.Property<Guid>("TakerOrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("taker_order_id");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id");

                    b.HasKey("Id");

                    b.HasIndex("MarketplaceOrderId");

                    b.HasIndex("TransactionId");

                    b.ToTable("order_matches");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.ReplacementTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("ErrorCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("error_code");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("error_msg");

                    b.Property<IEnumerable<TransactionModel>>("RawReplacementTransactions")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("raw_replacement_transactions");

                    b.Property<int>("ReplacementCounter")
                        .HasColumnType("integer")
                        .HasColumnName("replacement_counter");

                    b.Property<LocalDateTime>("RevertedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("reverted_date")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("transaction_id");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("replacement_transactions");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.ReplacementVaultUpdate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<BigInteger>("QuantizedAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<Guid>("ReplacementTransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("replacement_tx_id");

                    b.Property<Guid>("VaultId")
                        .HasColumnType("uuid")
                        .HasColumnName("vault_id");

                    b.HasKey("Id");

                    b.HasIndex("ReplacementTransactionId");

                    b.HasIndex("VaultId");

                    b.ToTable("replacement_vault_updates");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<LocalDateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("creation_date")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Operation")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("operation");

                    b.Property<TransactionModel>("RawTransaction")
                        .IsRequired()
                        .HasColumnType("jsonb")
                        .HasColumnName("raw_transaction");

                    b.Property<long>("StarkExTransactionId")
                        .HasColumnType("bigint")
                        .HasColumnName("starkex_tx_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Created")
                        .HasColumnName("status");

                    b.HasKey("Id");

                    b.HasIndex("StarkExTransactionId")
                        .IsUnique();

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<LocalDateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("creation_date")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("StarkKey")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("stark_key");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("StarkKey")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Vault", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<Guid>("AssetId")
                        .HasColumnType("uuid")
                        .HasColumnName("asset_id");

                    b.Property<string>("DataAvailabilityMode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("da_mode");

                    b.Property<Guid?>("MintingBlobId")
                        .HasColumnType("uuid")
                        .HasColumnName("minting_blob_id");

                    b.Property<Guid?>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.Property<BigInteger>("QuantizedAccountingBalance")
                        .HasColumnType("numeric")
                        .HasColumnName("accounting_balance");

                    b.Property<BigInteger>("QuantizedAvailableBalance")
                        .HasColumnType("numeric")
                        .HasColumnName("available_balance");

                    b.Property<string>("TokenId")
                        .HasColumnType("text")
                        .HasColumnName("token_id");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<BigInteger?>("VaultChainId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric")
                        .HasColumnName("vault_chain_id")
                        .HasDefaultValueSql("nextval('\"VaultChainId_ValidiumSequence\"')");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id");

                    b.HasIndex("AssetId");

                    b.HasIndex("MintingBlobId");

                    b.HasIndex("UserId");

                    b.HasIndex("VaultChainId")
                        .IsUnique();

                    b.ToTable("vaults");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.VaultUpdate", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<BigInteger>("QuantizedAmount")
                        .HasColumnType("numeric")
                        .HasColumnName("amount");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uuid")
                        .HasColumnName("tx_id");

                    b.Property<Guid>("VaultId")
                        .HasColumnType("uuid")
                        .HasColumnName("vault_id");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.HasIndex("VaultId");

                    b.ToTable("vault_updates");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.MintingBlobs.Erc1155MintingBlob", b =>
                {
                    b.HasBaseType("SevenSevenBit.Operator.Domain.Common.BaseMintingBlob");

                    b.ToTable("erc1155_minting_blobs");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.MintingBlobs.Erc20MintingBlob", b =>
                {
                    b.HasBaseType("SevenSevenBit.Operator.Domain.Common.BaseMintingBlob");

                    b.ToTable("erc20_minting_blobs");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.MintingBlobs.Erc721MintingBlob", b =>
                {
                    b.HasBaseType("SevenSevenBit.Operator.Domain.Common.BaseMintingBlob");

                    b.ToTable("erc721_minting_blobs");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Common.BaseMintingBlob", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Asset", "Asset")
                        .WithMany("MintingBlobs")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.Marketplace", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Asset", "BaseAsset")
                        .WithMany()
                        .HasForeignKey("BaseAssetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Asset", "QuoteAsset")
                        .WithMany()
                        .HasForeignKey("QuoteAssetId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("BaseAsset");

                    b.Navigation("QuoteAsset");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.MarketplaceOrder", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Vault", "BaseAssetVault")
                        .WithMany()
                        .HasForeignKey("BaseAssetVaultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Marketplace.Marketplace", "Marketplace")
                        .WithMany("Orders")
                        .HasForeignKey("MarketplaceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Vault", "QuoteAssetVault")
                        .WithMany()
                        .HasForeignKey("QuoteAssetVaultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.User", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserId1");

                    b.Navigation("BaseAssetVault");

                    b.Navigation("Marketplace");

                    b.Navigation("QuoteAssetVault");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.OrderMatch", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Marketplace.MarketplaceOrder", null)
                        .WithMany("Matches")
                        .HasForeignKey("MarketplaceOrderId");

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.ReplacementTransaction", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Transaction", "Transaction")
                        .WithMany("ReplacementTransactions")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.ReplacementVaultUpdate", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.ReplacementTransaction", "ReplacementTransaction")
                        .WithMany("ReplacementVaultUpdates")
                        .HasForeignKey("ReplacementTransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Vault", "Vault")
                        .WithMany("ReplacementVaultUpdates")
                        .HasForeignKey("VaultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ReplacementTransaction");

                    b.Navigation("Vault");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Vault", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Asset", "Asset")
                        .WithMany("Vaults")
                        .HasForeignKey("AssetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Common.BaseMintingBlob", "BaseMintingBlob")
                        .WithMany("Vaults")
                        .HasForeignKey("MintingBlobId");

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.User", "User")
                        .WithMany("Vaults")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Asset");

                    b.Navigation("BaseMintingBlob");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.VaultUpdate", b =>
                {
                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Transaction", "Transaction")
                        .WithMany("VaultUpdates")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SevenSevenBit.Operator.Domain.Entities.Vault", "Vault")
                        .WithMany("VaultUpdates")
                        .HasForeignKey("VaultId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transaction");

                    b.Navigation("Vault");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Common.BaseMintingBlob", b =>
                {
                    b.Navigation("Vaults");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Asset", b =>
                {
                    b.Navigation("MintingBlobs");

                    b.Navigation("Vaults");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.Marketplace", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Marketplace.MarketplaceOrder", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.ReplacementTransaction", b =>
                {
                    b.Navigation("ReplacementVaultUpdates");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Transaction", b =>
                {
                    b.Navigation("ReplacementTransactions");

                    b.Navigation("VaultUpdates");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.User", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("Vaults");
                });

            modelBuilder.Entity("SevenSevenBit.Operator.Domain.Entities.Vault", b =>
                {
                    b.Navigation("ReplacementVaultUpdates");

                    b.Navigation("VaultUpdates");
                });
#pragma warning restore 612, 618
        }
    }
}
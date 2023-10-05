using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.EntityFrameworkCore.Migrations;
using NodaTime;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;

#nullable disable

namespace SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData.Migrations.Code
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "VaultChainId_ValidiumSequence",
                maxValue: 2147483648L);

            migrationBuilder.CreateTable(
                name: "assets",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    starkex_type = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "text", nullable: false),
                    symbol = table.Column<string>(type: "text", nullable: false),
                    quantum = table.Column<BigInteger>(type: "numeric", nullable: false),
                    enabled = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assets", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "fees_config",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    action = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fees_config", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InboxState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiveCount = table.Column<int>(type: "integer", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxState", x => x.Id);
                    table.UniqueConstraint("AK_InboxState_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnqueueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Headers = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DestinationAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ResponseAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FaultAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.SequenceNumber);
                });

            migrationBuilder.CreateTable(
                name: "OutboxState",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxState", x => x.OutboxId);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    starkex_tx_id = table.Column<long>(type: "bigint", nullable: false),
                    raw_transaction = table.Column<TransactionModel>(type: "jsonb", nullable: false),
                    operation = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false, defaultValue: "Created"),
                    creation_date = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    stark_key = table.Column<string>(type: "text", nullable: false),
                    creation_date = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "erc1155_minting_blobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    vault_id = table.Column<Guid>(type: "uuid", nullable: true),
                    minting_blob = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<BigInteger>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_erc1155_minting_blobs", x => x.id);
                    table.ForeignKey(
                        name: "FK_erc1155_minting_blobs_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "erc20_minting_blobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    vault_id = table.Column<Guid>(type: "uuid", nullable: true),
                    minting_blob = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<BigInteger>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_erc20_minting_blobs", x => x.id);
                    table.ForeignKey(
                        name: "FK_erc20_minting_blobs_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "erc721_minting_blobs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    vault_id = table.Column<Guid>(type: "uuid", nullable: true),
                    minting_blob = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<BigInteger>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_erc721_minting_blobs", x => x.id);
                    table.ForeignKey(
                        name: "FK_erc721_minting_blobs_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "marketplaces",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    base_asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quote_asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    base_asset_token_id = table.Column<string>(type: "text", nullable: true),
                    quote_asset_token_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplaces", x => x.id);
                    table.ForeignKey(
                        name: "FK_marketplaces_assets_base_asset_id",
                        column: x => x.base_asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_marketplaces_assets_quote_asset_id",
                        column: x => x.quote_asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "replacement_transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    error_code = table.Column<string>(type: "text", nullable: false),
                    error_msg = table.Column<string>(type: "text", nullable: false),
                    reverted_date = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    replacement_counter = table.Column<int>(type: "integer", nullable: false),
                    raw_replacement_transactions = table.Column<IEnumerable<TransactionModel>>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_replacement_transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_replacement_transactions_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vaults",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    asset_id = table.Column<Guid>(type: "uuid", nullable: false),
                    minting_blob_id = table.Column<Guid>(type: "uuid", nullable: true),
                    vault_chain_id = table.Column<BigInteger>(type: "numeric", nullable: true, defaultValueSql: "nextval('\"VaultChainId_ValidiumSequence\"')"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: true),
                    token_id = table.Column<string>(type: "text", nullable: true),
                    da_mode = table.Column<string>(type: "text", nullable: false),
                    available_balance = table.Column<BigInteger>(type: "numeric", nullable: false),
                    accounting_balance = table.Column<BigInteger>(type: "numeric", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vaults", x => x.id);
                    table.ForeignKey(
                        name: "FK_vaults_assets_asset_id",
                        column: x => x.asset_id,
                        principalTable: "assets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vaults_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "marketplace_orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    marketplace_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    base_asset_vault_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quote_asset_vault_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    side = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    raw_order_model = table.Column<OrderRequestModel>(type: "jsonb", nullable: false),
                    UserId1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_marketplace_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_marketplace_orders_marketplaces_marketplace_id",
                        column: x => x.marketplace_id,
                        principalTable: "marketplaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_marketplace_orders_users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "users",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_marketplace_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_marketplace_orders_vaults_base_asset_vault_id",
                        column: x => x.base_asset_vault_id,
                        principalTable: "vaults",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_marketplace_orders_vaults_quote_asset_vault_id",
                        column: x => x.quote_asset_vault_id,
                        principalTable: "vaults",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "replacement_vault_updates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vault_id = table.Column<Guid>(type: "uuid", nullable: false),
                    replacement_tx_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<BigInteger>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_replacement_vault_updates", x => x.id);
                    table.ForeignKey(
                        name: "FK_replacement_vault_updates_replacement_transactions_replacem~",
                        column: x => x.replacement_tx_id,
                        principalTable: "replacement_transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_replacement_vault_updates_vaults_vault_id",
                        column: x => x.vault_id,
                        principalTable: "vaults",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vault_updates",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    vault_id = table.Column<Guid>(type: "uuid", nullable: false),
                    tx_id = table.Column<Guid>(type: "uuid", nullable: false),
                    amount = table.Column<BigInteger>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vault_updates", x => x.id);
                    table.ForeignKey(
                        name: "FK_vault_updates_transactions_tx_id",
                        column: x => x.tx_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vault_updates_vaults_vault_id",
                        column: x => x.vault_id,
                        principalTable: "vaults",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_matches",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    maker_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    taker_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    quantity = table.Column<BigInteger>(type: "numeric", nullable: false),
                    price = table.Column<BigInteger>(type: "numeric", nullable: false),
                    created_at = table.Column<LocalDateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    transaction_id = table.Column<Guid>(type: "uuid", nullable: false),
                    MarketplaceOrderId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_matches", x => x.id);
                    table.ForeignKey(
                        name: "FK_order_matches_marketplace_orders_MarketplaceOrderId",
                        column: x => x.MarketplaceOrderId,
                        principalTable: "marketplace_orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_order_matches_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assets_starkex_type",
                table: "assets",
                column: "starkex_type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_erc1155_minting_blobs_asset_id_minting_blob",
                table: "erc1155_minting_blobs",
                columns: new[] { "asset_id", "minting_blob" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_erc20_minting_blobs_asset_id_minting_blob",
                table: "erc20_minting_blobs",
                columns: new[] { "asset_id", "minting_blob" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_erc721_minting_blobs_asset_id_minting_blob",
                table: "erc721_minting_blobs",
                columns: new[] { "asset_id", "minting_blob" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_fees_config_action",
                table: "fees_config",
                column: "action",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InboxState_Delivered",
                table: "InboxState",
                column: "Delivered");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_orders_base_asset_vault_id",
                table: "marketplace_orders",
                column: "base_asset_vault_id");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_orders_marketplace_id",
                table: "marketplace_orders",
                column: "marketplace_id");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_orders_quote_asset_vault_id",
                table: "marketplace_orders",
                column: "quote_asset_vault_id");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_orders_user_id",
                table: "marketplace_orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_marketplace_orders_UserId1",
                table: "marketplace_orders",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_marketplaces_base_asset_id",
                table: "marketplaces",
                column: "base_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_marketplaces_quote_asset_id",
                table: "marketplaces",
                column: "quote_asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_order_matches_MarketplaceOrderId",
                table: "order_matches",
                column: "MarketplaceOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_order_matches_transaction_id",
                table: "order_matches",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_EnqueueTime",
                table: "OutboxMessage",
                column: "EnqueueTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ExpirationTime",
                table: "OutboxMessage",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "InboxMessageId", "InboxConsumerId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_Created",
                table: "OutboxState",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_replacement_transactions_transaction_id",
                table: "replacement_transactions",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "IX_replacement_vault_updates_replacement_tx_id",
                table: "replacement_vault_updates",
                column: "replacement_tx_id");

            migrationBuilder.CreateIndex(
                name: "IX_replacement_vault_updates_vault_id",
                table: "replacement_vault_updates",
                column: "vault_id");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_starkex_tx_id",
                table: "transactions",
                column: "starkex_tx_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_stark_key",
                table: "users",
                column: "stark_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_vault_updates_tx_id",
                table: "vault_updates",
                column: "tx_id");

            migrationBuilder.CreateIndex(
                name: "IX_vault_updates_vault_id",
                table: "vault_updates",
                column: "vault_id");

            migrationBuilder.CreateIndex(
                name: "IX_vaults_asset_id",
                table: "vaults",
                column: "asset_id");

            migrationBuilder.CreateIndex(
                name: "IX_vaults_minting_blob_id",
                table: "vaults",
                column: "minting_blob_id");

            migrationBuilder.CreateIndex(
                name: "IX_vaults_user_id",
                table: "vaults",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_vaults_vault_chain_id",
                table: "vaults",
                column: "vault_chain_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "erc1155_minting_blobs");

            migrationBuilder.DropTable(
                name: "erc20_minting_blobs");

            migrationBuilder.DropTable(
                name: "erc721_minting_blobs");

            migrationBuilder.DropTable(
                name: "fees_config");

            migrationBuilder.DropTable(
                name: "InboxState");

            migrationBuilder.DropTable(
                name: "order_matches");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "OutboxState");

            migrationBuilder.DropTable(
                name: "replacement_vault_updates");

            migrationBuilder.DropTable(
                name: "vault_updates");

            migrationBuilder.DropTable(
                name: "marketplace_orders");

            migrationBuilder.DropTable(
                name: "replacement_transactions");

            migrationBuilder.DropTable(
                name: "marketplaces");

            migrationBuilder.DropTable(
                name: "vaults");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "assets");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropSequence(
                name: "VaultChainId_ValidiumSequence");
        }
    }
}

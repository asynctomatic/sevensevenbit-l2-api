#nullable disable

namespace SevenSevenBit.Operator.Infrastructure.SQL.Data.BlockchainData.Migrations.Code;

using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

public partial class InitialSetup : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AddressTransactions",
            columns: table => new
            {
                RowIndex = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                BlockNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Hash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: false),
                Address = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: false),
                RowCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RowUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AddressTransactions", x => x.RowIndex);
            });

        migrationBuilder.CreateTable(
            name: "BlockProgress",
            columns: table => new
            {
                RowIndex = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                LastBlockProcessed = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: false),
                RowCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RowUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_BlockProgress", x => x.RowIndex);
            });

        migrationBuilder.CreateTable(
            name: "Blocks",
            columns: table => new
            {
                RowIndex = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                BlockNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Hash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: false),
                ParentHash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: false),
                Nonce = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                ExtraData = table.Column<string>(type: "text", nullable: true),
                Difficulty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                TotalDifficulty = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Size = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Miner = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                GasLimit = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                GasUsed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Timestamp = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                TransactionCount = table.Column<long>(type: "bigint", nullable: false),
                BaseFeePerGas = table.Column<string>(type: "text", nullable: true),
                RowCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RowUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Blocks", x => x.RowIndex);
            });

        migrationBuilder.CreateTable(
            name: "Contracts",
            columns: table => new
            {
                RowIndex = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Address = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                ABI = table.Column<string>(type: "text", nullable: true),
                Code = table.Column<string>(type: "text", nullable: true),
                Creator = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                TransactionHash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                RowCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RowUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Contracts", x => x.RowIndex);
            });

        migrationBuilder.CreateTable(
            name: "TransactionLogs",
            columns: table => new
            {
                RowIndex = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                TransactionHash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: false),
                LogIndex = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Address = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                EventHash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                IndexVal1 = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                IndexVal2 = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                IndexVal3 = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                Data = table.Column<string>(type: "text", nullable: true),
                RowCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RowUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TransactionLogs", x => x.RowIndex);
            });

        migrationBuilder.CreateTable(
            name: "TransactionLogVmStacks",
            columns: table => new
            {
                RowIndex = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Address = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                TransactionHash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                StructLogs = table.Column<string>(type: "text", nullable: true),
                RowCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RowUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_TransactionLogVmStacks", x => x.RowIndex);
            });

        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                RowIndex = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                RowCreated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                RowUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                BlockHash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                BlockNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Hash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: false),
                AddressFrom = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                TimeStamp = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                TransactionIndex = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                AddressTo = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                Gas = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                GasPrice = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Input = table.Column<string>(type: "text", nullable: true),
                Nonce = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                Failed = table.Column<bool>(type: "boolean", nullable: false),
                ReceiptHash = table.Column<string>(type: "character varying(67)", maxLength: 67, nullable: true),
                GasUsed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                CumulativeGasUsed = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                EffectiveGasPrice = table.Column<string>(type: "text", nullable: true),
                HasLog = table.Column<bool>(type: "boolean", nullable: false),
                Error = table.Column<string>(type: "text", nullable: true),
                HasVmStack = table.Column<bool>(type: "boolean", nullable: false),
                NewContractAddress = table.Column<string>(type: "character varying(43)", maxLength: 43, nullable: true),
                FailedCreateContract = table.Column<bool>(type: "boolean", nullable: false),
                MaxFeePerGas = table.Column<string>(type: "text", nullable: true),
                MaxPriorityFeePerGas = table.Column<string>(type: "text", nullable: true),
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Transactions", x => x.RowIndex);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AddressTransactions_Address",
            table: "AddressTransactions",
            column: "Address");

        migrationBuilder.CreateIndex(
            name: "IX_AddressTransactions_BlockNumber_Hash_Address",
            table: "AddressTransactions",
            columns: new[] { "BlockNumber", "Hash", "Address" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_AddressTransactions_Hash",
            table: "AddressTransactions",
            column: "Hash");

        migrationBuilder.CreateIndex(
            name: "IX_BlockProgress_LastBlockProcessed",
            table: "BlockProgress",
            column: "LastBlockProcessed");

        migrationBuilder.CreateIndex(
            name: "IX_Blocks_BlockNumber_Hash",
            table: "Blocks",
            columns: new[] { "BlockNumber", "Hash" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Contracts_Address",
            table: "Contracts",
            column: "Address");

        migrationBuilder.CreateIndex(
            name: "IX_Contracts_Name",
            table: "Contracts",
            column: "Name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogs_Address",
            table: "TransactionLogs",
            column: "Address");

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogs_EventHash",
            table: "TransactionLogs",
            column: "EventHash");

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogs_IndexVal1",
            table: "TransactionLogs",
            column: "IndexVal1");

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogs_IndexVal2",
            table: "TransactionLogs",
            column: "IndexVal2");

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogs_IndexVal3",
            table: "TransactionLogs",
            column: "IndexVal3");

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogs_TransactionHash_LogIndex",
            table: "TransactionLogs",
            columns: new[] { "TransactionHash", "LogIndex" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogVmStacks_Address",
            table: "TransactionLogVmStacks",
            column: "Address");

        migrationBuilder.CreateIndex(
            name: "IX_TransactionLogVmStacks_TransactionHash",
            table: "TransactionLogVmStacks",
            column: "TransactionHash");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_AddressFrom",
            table: "Transactions",
            column: "AddressFrom");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_AddressTo",
            table: "Transactions",
            column: "AddressTo");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_BlockNumber_Hash",
            table: "Transactions",
            columns: new[] { "BlockNumber", "Hash" },
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_Hash",
            table: "Transactions",
            column: "Hash");

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_NewContractAddress",
            table: "Transactions",
            column: "NewContractAddress");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AddressTransactions");

        migrationBuilder.DropTable(
            name: "BlockProgress");

        migrationBuilder.DropTable(
            name: "Blocks");

        migrationBuilder.DropTable(
            name: "Contracts");

        migrationBuilder.DropTable(
            name: "TransactionLogs");

        migrationBuilder.DropTable(
            name: "TransactionLogVmStacks");

        migrationBuilder.DropTable(
            name: "Transactions");
    }
}
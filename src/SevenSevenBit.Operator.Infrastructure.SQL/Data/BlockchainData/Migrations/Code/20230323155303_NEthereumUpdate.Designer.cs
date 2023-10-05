﻿// <auto-generated />

#nullable disable

namespace SevenSevenBit.Operator.Infrastructure.SQL.Data.BlockchainData.Migrations.Code
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Migrations;

    [DbContext(typeof(BlockchainDbContext))]
    [Migration("20230323155303_NEthereumUpdate")]
    partial class NEthereumUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Nethereum.BlockchainProcessing.BlockStorage.Entities.AddressTransaction", b =>
                {
                    b.Property<int>("RowIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RowIndex"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("BlockNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<DateTime?>("RowCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RowUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("RowIndex");

                    b.HasIndex("Address");

                    b.HasIndex("Hash");

                    b.HasIndex("BlockNumber", "Hash", "Address")
                        .IsUnique();

                    b.ToTable("AddressTransactions", (string)null);
                });

            modelBuilder.Entity("Nethereum.BlockchainProcessing.BlockStorage.Entities.Block", b =>
                {
                    b.Property<int>("RowIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RowIndex"));

                    b.Property<string>("BaseFeePerGas")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("BlockNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Difficulty")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ExtraData")
                        .HasColumnType("text");

                    b.Property<string>("GasLimit")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("GasUsed")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<string>("Miner")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("Nonce")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ParentHash")
                        .IsRequired()
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<DateTime?>("RowCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RowUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Size")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Timestamp")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("TotalDifficulty")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<long>("TransactionCount")
                        .HasColumnType("bigint");

                    b.HasKey("RowIndex");

                    b.HasIndex("BlockNumber", "Hash")
                        .IsUnique();

                    b.ToTable("Blocks", (string)null);
                });

            modelBuilder.Entity("Nethereum.BlockchainProcessing.BlockStorage.Entities.BlockProgress", b =>
                {
                    b.Property<int>("RowIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RowIndex"));

                    b.Property<string>("LastBlockProcessed")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("RowCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RowUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("RowIndex");

                    b.HasIndex("LastBlockProcessed");

                    b.ToTable("BlockProgress", (string)null);
                });

            modelBuilder.Entity("Nethereum.BlockchainProcessing.BlockStorage.Entities.Contract", b =>
                {
                    b.Property<int>("RowIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RowIndex"));

                    b.Property<string>("ABI")
                        .HasColumnType("text");

                    b.Property<string>("Address")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("Code")
                        .HasColumnType("text");

                    b.Property<string>("Creator")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime?>("RowCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RowUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TransactionHash")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.HasKey("RowIndex");

                    b.HasIndex("Address");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Contracts", (string)null);
                });

            modelBuilder.Entity("Nethereum.BlockchainProcessing.BlockStorage.Entities.Transaction", b =>
                {
                    b.Property<int>("RowIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RowIndex"));

                    b.Property<string>("AddressFrom")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("AddressTo")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("BlockHash")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<string>("BlockNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("CumulativeGasUsed")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("EffectiveGasPrice")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Error")
                        .HasColumnType("text");

                    b.Property<bool>("Failed")
                        .HasColumnType("boolean");

                    b.Property<bool>("FailedCreateContract")
                        .HasColumnType("boolean");

                    b.Property<string>("Gas")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("GasPrice")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("GasUsed")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("HasLog")
                        .HasColumnType("boolean");

                    b.Property<bool>("HasVmStack")
                        .HasColumnType("boolean");

                    b.Property<string>("Hash")
                        .IsRequired()
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<string>("Input")
                        .HasColumnType("text");

                    b.Property<string>("MaxFeePerGas")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("MaxPriorityFeePerGas")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("NewContractAddress")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("Nonce")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("ReceiptHash")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<DateTime?>("RowCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RowUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TimeStamp")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("TransactionIndex")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Value")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("RowIndex");

                    b.HasIndex("AddressFrom");

                    b.HasIndex("AddressTo");

                    b.HasIndex("Hash");

                    b.HasIndex("NewContractAddress");

                    b.HasIndex("BlockNumber", "Hash")
                        .IsUnique();

                    b.ToTable("Transactions", (string)null);
                });

            modelBuilder.Entity("Nethereum.BlockchainProcessing.BlockStorage.Entities.TransactionLog", b =>
                {
                    b.Property<int>("RowIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RowIndex"));

                    b.Property<string>("Address")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<string>("EventHash")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<string>("IndexVal1")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<string>("IndexVal2")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<string>("IndexVal3")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.Property<string>("LogIndex")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("RowCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RowUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("TransactionHash")
                        .IsRequired()
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.HasKey("RowIndex");

                    b.HasIndex("Address");

                    b.HasIndex("EventHash");

                    b.HasIndex("IndexVal1");

                    b.HasIndex("IndexVal2");

                    b.HasIndex("IndexVal3");

                    b.HasIndex("TransactionHash", "LogIndex")
                        .IsUnique();

                    b.ToTable("TransactionLogs", (string)null);
                });

            modelBuilder.Entity("Nethereum.BlockchainProcessing.BlockStorage.Entities.TransactionVmStack", b =>
                {
                    b.Property<int>("RowIndex")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("RowIndex"));

                    b.Property<string>("Address")
                        .HasMaxLength(43)
                        .HasColumnType("character varying(43)");

                    b.Property<DateTime?>("RowCreated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("RowUpdated")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("StructLogs")
                        .HasColumnType("text");

                    b.Property<string>("TransactionHash")
                        .HasMaxLength(67)
                        .HasColumnType("character varying(67)");

                    b.HasKey("RowIndex");

                    b.HasIndex("Address");

                    b.HasIndex("TransactionHash");

                    b.ToTable("TransactionLogVmStacks", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}

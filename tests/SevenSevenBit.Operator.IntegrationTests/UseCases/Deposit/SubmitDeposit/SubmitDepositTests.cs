namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Deposit.SubmitDeposit;

using System.Globalization;
using System.Numerics;
using Nethereum.Contracts;
using Nethereum.Web3.Accounts;
using SevenSevenBit.Operator.Application.Blockchain.Functions;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.Fixture.Blockchain;
using SevenSevenBit.Operator.IntegrationTests.UseCases.Deposit.SubmitDeposit.Data;
using SevenSevenBit.Operator.TestHelpers.Data;
using StarkEx.Crypto.SDK.Encoding;
using Xunit;

[Collection("Worker Integration Tests")]
public class SubmitDepositTests : IAsyncLifetime
{
    // TODO: Get token admin private key from a config file.
    private const string TokenAdminPrivateKey = "5af96d362dfe2b0878ac44fa88ac65c5f7d90e3939fb40baa328f414a18933d4";
    private const string StarkExContractAddress = "0x5fbdb2315678afecb367f032d93f642f64180aa3";

    private readonly OperatorWorkerFactory worker;
    private readonly DatabaseFixture database;
    private readonly BlockchainFixture blockchain;

    public SubmitDepositTests(
        OperatorWorkerFactory worker,
        DatabaseFixture database,
        BlockchainFixture blockchain)
    {
        this.worker = worker;
        this.database = database;
        this.blockchain = blockchain;
    }

    public async Task InitializeAsync()
    {
        // Seed the database.
        var unitOfWork = new UnitOfWork(database.Context);  // TODO: Move to db fixture class.
        await unitOfWork.Repository<User>().InsertAsync(Users.GetUsers(), CancellationToken.None);
        await unitOfWork.Repository<Asset>().InsertAsync(Assets.GetAssets(), CancellationToken.None);
        // await unitOfWork.Repository<Vault>().InsertAsync(Vaults.GetVaults(), CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);

        // Seed the blockchain with assets.
        var txSender = new Account(TokenAdminPrivateKey);
        foreach (var asset in Assets.GetAssets())
        {
            await blockchain.SendTransactionAsync(
                new RegisterTokenFunction
                {
                    // TODO: The StarkExType should have a native conversion to BigInteger.
                    AssetType = BigInteger.Parse(asset.StarkExType.Value[2..], NumberStyles.AllowHexSpecifier),
                    // TODO: The AssetInfo should be a native type and be part of an Asset.
                    AssetInfo = AssetEncoder.GetAssetInfo(asset.Type, asset.Address).ToByteArrayUnsigned(),
                    Quantum = asset.Quantum.Value,
                },
                txSender,
                StarkExContractAddress); // TODO: The StarkEx contract should be read from a config file.
        }
    }

    public async Task DisposeAsync()
    {
        await database.ResetAsync();
        await blockchain.ResetAsync();
    }

    [Theory]
    [ClassData(typeof(SuccessTestData))]
    public async Task SubmitDeposit_HappyPath_DepositIsProcessed(FunctionMessage transaction)
    {
        // Arrange
        var txSender = new Account(TokenAdminPrivateKey);

        // Act
        // Submit the deposit transaction to the blockchain.
        await blockchain.SendTransactionAsync(
            transaction,
            txSender,
            StarkExContractAddress);

        // Wait for N block confirmations.
        // TODO: options.Value.MinimumBlockConfirmations + 1
        await blockchain.MineBlocksAsync(12 + 1);

        // Wait for the worker to process the deposit.
        await Task.Delay(1000, CancellationToken.None);

        // Assert
        var unitOfWork = new UnitOfWork(database.Context);
    }
}
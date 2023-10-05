namespace SevenSevenBit.Operator.IntegrationTests.Fixture.Blockchain;

using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Nethereum.Contracts;
using Nethereum.RPC.Accounts;
using Nethereum.Web3;
using SevenSevenBit.Operator.TestHelpers.Blockchain.Functions.Anvil;
using Xunit;

public class BlockchainFixture : IAsyncLifetime
{
    private readonly IContainer container;

    private IWeb3 web3;
    private string snapshot;

    public BlockchainFixture()
    {
        // TODO: Enable this.
        // container = new ContainerBuilder()
        //     .WithImage("threesigmalabs.azurecr.io/threesigmaxyz/threesigma-starkex-contracts:latest")
        //     .WithPortBinding(8545)
        //     .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("cast chain-id"))
        //     .WithCleanUp(true)
        //     .Build();
    }

    public async Task SendTransactionAsync<T>(T transaction, IAccount sender, string contract)
        where T : FunctionMessage, new()
    {
        var signer = new Web3(sender, web3.Client);
        signer.TransactionManager.Fee1559SuggestionStrategy = new ConstantFeeSuggestionStrategy();

        var handler = signer.Eth.GetContractTransactionHandler<T>();

        await handler.SendRequestAndWaitForReceiptAsync(
            contract, transaction, CancellationToken.None);
    }

    public async Task MineBlocksAsync(uint numBlocks)
    {
        await new AnvilMine(web3.Client).SendRequestAsync(numBlocks);
    }

    public async Task InitializeAsync()
    {
        // TODO: Enable this.
        /*
        // Start the container.
        await container.StartAsync();

        // Build and tag the blockchain migrations container image.
        var migrationsContainerImage = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(CommonDirectoryPath.GetSolutionDirectory(), string.Empty)
            .WithDockerfile("Dockerfile")
            .Build();
        await migrationsContainerImage.CreateAsync().ConfigureAwait(false);

        // Initialize the blockchain migrations container and apply migrations.
        var migrationsContainer = new ContainerBuilder()
            .WithImage(migrationsContainerImage.Tag)
            .WithPortBinding(8545)
            .WithEnvironment(new Dictionary<string, string>
            {
                { "STARKEX_SEQUENCE_NUMBER", "1" },
                { "STARKEX_VALIDIUM_VAULT_ROOT", "2080691150869914534766879727318932669398085814729621863115884828556130066692" },
                { "STARKEX_VALIDIUM_TREE_HEIGHT", "31" },
                { "STARKEX_ROLLUP_VAULT_ROOT", "207095555137602068174310225607660532858489993604082708018689543482077973596" },
                { "STARKEX_ROLLUP_TREE_HEIGHT", "31" },
                { "STARKEX_ORDER_ROOT", "782789488582197453756570607249782803464646337934052302582063579083846343149" },
                { "STARKEX_ORDER_TREE_HEIGHT", "255" },
                { "STARKEX_STRICT_VAULT_BALANCE_POLICY", "false" },
                { "STARKEX_TOKEN_ADMIN", "0xf39fd6e51aad88f6f4ce6ab8827279cfffb92266" },
                { "STARKEX_DA_THRESHOLD", "0" },
                { "SCALABLE_DEX_ADDRESS", "0x5fbdb2315678afecb367f032d93f642f64180aa3" },
                { "FAUCET_ADDRESS", "0x5C3cB0E1fe0b525f9A3db41286Bab06A39977639" },
            })
            .WithWaitStrategy(Wait.ForUnixContainer().UntilCommandIsCompleted("YOUR_WAIT_COMMAND_HERE"))
            .DependsOn(container)
            .Build();

        await migrationsContainer.StartAsync();*/

        // TODO: seed blockchain data.

        // TODO: hard-coded rpc-endpoint
        web3 = new Web3("http://localhost:8545");
        // web3.TransactionManager.Fee1559SuggestionStrategy = new MedianPriorityFeeHistorySuggestionStrategy(web3.Client);    // TODO: no web3.Client in constructor.
        // web3.TransactionManager.Fee1559SuggestionStrategy = new MedianPriorityFeeHistorySuggestionStrategy();
        // await MineBlocksAsync(100); // TODO: The default strategy takes the historical gas price from the last 100 blocks.

        // Snapshot the state of the blockchain.
        snapshot = await new EvmSnapshot(web3.Client).SendRequestAsync();
        if (!snapshot.Equals("0x0"))
        {
            // There is a previous snapshot, so revert to it.
            snapshot = "0x0";
        }
    }

    public async Task DisposeAsync()
    {
        // Stop the container.
        // TODO: enable this. await container.StopAsync();
    }

    public async Task ResetAsync()
    {
        // Restore the blockchain snapshot.
        await new EvmRevert(web3.Client).SendRequestAsync(snapshot);
    }
}
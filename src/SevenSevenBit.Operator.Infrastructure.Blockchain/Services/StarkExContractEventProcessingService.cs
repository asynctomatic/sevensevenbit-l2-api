namespace SevenSevenBit.Operator.Infrastructure.Blockchain.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nethereum.BlockchainProcessing.Processor;
using Nethereum.BlockchainProcessing.ProgressRepositories;
using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using SevenSevenBit.Operator.Application.Blockchain.Events;
using SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;
using SevenSevenBit.Operator.Infrastructure.Blockchain.Options;
using SevenSevenBit.Operator.SharedKernel.Telemetry.Attributes;

public class StarkExContractEventProcessingService : BackgroundService
{
    private readonly ILogger<StarkExContractEventProcessingService> logger;
    private readonly IWeb3 web3;
    private readonly IBlockProgressRepositoryFactory blockProgressRepositoryFactory;
    private readonly IServiceProvider serviceProvider;
    private readonly BlockchainOptions blockchainOptions;

    public StarkExContractEventProcessingService(
        ILogger<StarkExContractEventProcessingService> logger,
        IWeb3 web3,
        IBlockProgressRepositoryFactory blockProgressRepositoryFactory,
        IOptions<BlockchainOptions> blockchainOptions,
        IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.web3 = web3;
        this.blockProgressRepositoryFactory = blockProgressRepositoryFactory;
        this.serviceProvider = serviceProvider;
        this.blockchainOptions = blockchainOptions.Value;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogWarning($"Stopping {nameof(StarkExContractEventProcessingService)}");

        await base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var nrOfRetries = 0;

        while (!stoppingToken.IsCancellationRequested && nrOfRetries <= blockchainOptions.RetryAttemptsBetweenWorkerRestarts)
        {
            try
            {
                // List with a default contract address
                var contracts = new List<string>
                {
                    blockchainOptions.StarkExContractAddress,
                };

                var processor = web3.Processing.Logs.CreateProcessor(
                    new[]
                    {
                        new ProcessorHandler<FilterLog>(
                            async filterLog => await ProcessLogAsync(filterLog, stoppingToken)),
                    },
                    blockchainOptions.MinimumBlockConfirmations,
                    new NewFilterInput
                    {
                        Address = contracts.Select(blockchainAddress => blockchainAddress).ToArray(),
                    },
                    blockProgressRepositoryFactory.CreateBlockProgressRepository(),
                    logger,
                    blockchainOptions.NumberOfBlocksPerRequest);

                var startAtBlockNumber = 0; // TODO: starkExInstances.GetLowestDeploymentBlockNumber();

                await processor.ExecuteAsync(
                    cancellationToken: stoppingToken,
                    startAtBlockNumberIfNotProcessed: startAtBlockNumber,
                    waitInterval: blockchainOptions.WaitIntervalInMsBetweenRequests);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Stopped listening for StarkEx events");
            }
            catch (Exception exception)
            {
                logger.LogError(
                    exception,
                    "An exception was thrown while listening for StarkEx events");
            }

            // Wait some time for the transient failures to be dealt until the number of max retry attempts is reached
            nrOfRetries++;
            await Task.Delay(blockchainOptions.WaitIntervalInMsBetweenWorkerCrashes, stoppingToken);
        }

        // Crash application to force a restart by Kubernetes
        Environment.Exit(-1);
    }

    [Transaction(Web = false)]
    private async Task ProcessLogAsync(
        FilterLog log,
        CancellationToken cancellationToken)
    {
        var scope = serviceProvider.CreateScope();
        var starkExEventHandlerService = scope.ServiceProvider.GetRequiredService<IStarkExEventHandlerService>();

        if (log.IsLogForEvent<LogDepositEvent>())
        {
            await starkExEventHandlerService.HandleLogDepositEventAsync(
                log.DecodeEvent<LogDepositEvent>(), cancellationToken);
        }
        else if (log.IsLogForEvent<LogDepositWithTokenIdEvent>())
        {
            await starkExEventHandlerService.HandleLogDepositWithTokenIdEventAsync(
                log.DecodeEvent<LogDepositWithTokenIdEvent>(), cancellationToken);
        }
        else if (log.IsLogForEvent<LogDepositCancelEvent>())
        {
            starkExEventHandlerService.HandleLogDepositCancelEvent(log.DecodeEvent<LogDepositCancelEvent>());
        }
        else if (log.IsLogForEvent<LogDepositCancelReclaimedEvent>())
        {
            starkExEventHandlerService.HandleLogDepositCancelReclaimedEvent(log.DecodeEvent<LogDepositCancelReclaimedEvent>());
        }
        else if (log.IsLogForEvent<LogDepositWithTokenIdCancelReclaimedEvent>())
        {
            starkExEventHandlerService.HandleLogDepositWithTokenIdCancelReclaimedEvent(log.DecodeEvent<LogDepositWithTokenIdCancelReclaimedEvent>());
        }
        else if (log.IsLogForEvent<LogFullWithdrawalRequestEvent>())
        {
            await starkExEventHandlerService.HandleLogFullWithdrawalRequestEventAsync(
                log.DecodeEvent<LogFullWithdrawalRequestEvent>(), cancellationToken);
        }
    }
}
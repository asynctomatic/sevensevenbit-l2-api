namespace SevenSevenBit.Operator.Infrastructure.StarkExApi.Services;

using Microsoft.Extensions.Logging;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Constants;
using StarkEx.Client.SDK.Exceptions;
using StarkEx.Client.SDK.Interfaces.Spot;
using StarkEx.Client.SDK.Models.Spot.RequestModels;
using StarkEx.Client.SDK.Models.Spot.TransactionModels;
using StarkEx.Client.SDK.Settings;

/// <inheritdoc />
public class StarkExApiGateway : IStarkExApiGateway
{
    private readonly ILogger<StarkExApiGateway> logger;
    private readonly ISpotGatewayClient spotGatewayClient;
    private readonly IFactory<StarkExApiSettings, ISpotGatewayClient> spotGatewayClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="StarkExApiGateway"/> class.
    /// </summary>
    /// <param name="logger">An instance of the <see cref="ILogger{StarkExService}"/> interface for logging.</param>
    /// <param name="spotGatewayClient">StarkEx Spot Gateway client instantiated from runtime settings.</param>
    /// <param name="spotGatewayClientFactory">StarkEx Spot Gateway client factory to generate clients for the StarkEx SPOT Api.</param>
    public StarkExApiGateway(
        ILogger<StarkExApiGateway> logger,
        ISpotGatewayClient spotGatewayClient,
        IFactory<StarkExApiSettings, ISpotGatewayClient> spotGatewayClientFactory)
    {
        this.logger = logger;
        this.spotGatewayClient = spotGatewayClient;
        this.spotGatewayClientFactory = spotGatewayClientFactory;
    }

    /// <inheritdoc />
    public async Task SendTransactionAsync(
        Uri starkExUri,
        string starkExVersion,
        TransactionModel transaction,
        long transactionId,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new RequestModel
            {
                Transaction = transaction,
                TransactionId = transactionId,
            };

            var gatewayClient = spotGatewayClientFactory.Create(
                new StarkExApiSettings
                {
                    BaseAddress = starkExUri,
                    Version = starkExVersion,
                    HttpSpotClientName = HttpConstants.HttpStarkExClientName,
                });

            await gatewayClient.AddTransactionAsync(request, cancellationToken);
        }
        catch (StarkExErrorException ex)
        {
            logger.LogError(
                "Invalid StarkEx API code {Code} for transaction with id {TxId} with message: {Message} and problems: {Problems}",
                ex.Code,
                transactionId,
                ex.Message,
                ex.Problems);

            throw;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            logger.LogError(
                "Failed HTTP request for transaction with id {TxId}",
                transactionId);

            throw;
        }
    }

    public async Task<int> GetFirstUnusedTxAsync(CancellationToken cancellationToken)
    {
        return await spotGatewayClient.GetFirstUnusedTxAsync(cancellationToken);
    }
}
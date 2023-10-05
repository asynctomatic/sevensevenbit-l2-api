namespace SevenSevenBit.Operator.Application.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Application.Options;

/// <summary>
/// A class that implements the <see cref="ITransactionIdService"/> interface.
/// It utilizes the provided Redis connection to store and retrieve the transaction ID values.
/// Complex atomic Redis operations rely the execution of <see href="https://redis.io/docs/manual/programmability/eval-intro/">Lua scripts</see>.
/// </summary>
public class TransactionIdService : ITransactionIdService
{
    private readonly FeatureTogglesOptions featureToggleOptions;
    private readonly ILogger<TransactionIdService> logger;
    private readonly INoSqlService noSqlService;
    private readonly IStarkExApiGateway starkExApiGateway;

    /// <summary>
    /// Initializes a new instance of the <see cref="TransactionIdService"/> class.
    /// </summary>
    /// <param name="featureToggleOptions">Feature toggle settings.</param>
    /// <param name="logger">Logger.</param>
    /// <param name="noSqlService">A service to interact with Redis.</param>
    /// <param name="starkExApiGateway">Service for the StarkEx API.</param>
    public TransactionIdService(
        IOptions<FeatureTogglesOptions> featureToggleOptions,
        ILogger<TransactionIdService> logger,
        INoSqlService noSqlService,
        IStarkExApiGateway starkExApiGateway)
    {
        this.featureToggleOptions = featureToggleOptions.Value;
        this.logger = logger;
        this.noSqlService = noSqlService;
        this.starkExApiGateway = starkExApiGateway;
    }

    /// <inheritdoc />
    /// <remarks>
    /// Caches the assigned ID and returns it for any subsequent request with the same parameters.
    /// </remarks>
    public async Task<long> AllocateTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId, CancellationToken cancellationToken)
    {
        var transactionId = featureToggleOptions.GenerateTransactionIdFromStarkExApi
            ? await starkExApiGateway.GetFirstUnusedTxAsync(cancellationToken)
            : await noSqlService.LockTransactionIdAsync(transactionKey, starkExInstanceId);

        logger.LogInformation(
            "Allocated transaction id {TxId} for transaction key {TxKey} in StarkEx instance {StarkExInstanceId}",
            transactionId,
            transactionKey,
            starkExInstanceId);

        return transactionId;
    }

    /// <inheritdoc />
    public async Task FreeTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId)
    {
        logger.LogInformation(
            "Freed transaction id for transaction key {TxKey} in StarkEx instance {StarkExInstanceId}",
            transactionKey,
            starkExInstanceId);

        // Execute the parametrized unlock Redis Lua script.
        await noSqlService.UnlockTransactionIdAsync(transactionKey, starkExInstanceId);
    }
}
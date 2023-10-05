namespace SevenSevenBit.Operator.Infrastructure.Blockchain.Options;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents configuration options for interacting with the blockchain.
/// </summary>
[ExcludeFromCodeCoverage]
public record BlockchainOptions
{
    /// <summary>
    /// The configuration key for accessing the blockchain options in a configuration file.
    /// </summary>
    public const string Blockchain = "Services:Blockchain";

    /// <summary>
    /// Gets the RPC node endpoint.
    /// </summary>
    public string RpcEndpoint { get; init; }

    /// <summary>
    /// Gets the minimum number of block confirmations required for a transaction to be considered confirmed.
    /// </summary>
    public uint MinimumBlockConfirmations { get; init; }

    /// <summary>
    /// Gets a value indicating whether to use the default fee suggestion strategy for transactions.
    /// </summary>
    public bool UseDefaultFeeSuggestionStrategy { get; init; }

    /// <summary>
    /// Gets the number of blocks to retrieve per request.
    /// </summary>
    public int NumberOfBlocksPerRequest { get; init; }

    /// <summary>
    /// Gets the wait interval between requests to the blockchain in milliseconds.
    /// </summary>
    public int WaitIntervalInMsBetweenRequests { get; init; }

    /// <summary>
    /// Gets the wait interval between new attempts to restart the event processing background worker.
    /// </summary>
    public int WaitIntervalInMsBetweenWorkerCrashes { get; init; }

    /// <summary>
    /// Gets the max number of retries between restarts for the event processing background worker.
    /// </summary>
    public int RetryAttemptsBetweenWorkerRestarts { get; init; }

    /// <summary>
    /// Gets the address of the StarkEx contract.
    /// </summary>
    public string StarkExContractAddress { get; init; }
}
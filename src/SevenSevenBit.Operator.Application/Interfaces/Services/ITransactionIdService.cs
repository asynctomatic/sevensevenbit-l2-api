namespace SevenSevenBit.Operator.Application.Interfaces.Services;

/// <summary>
/// A service interface for managing the allocation and freeing of transaction IDs for the StarkEx service.
/// </summary>
public interface ITransactionIdService
{
    /// <summary>
    /// Allocates a new transaction ID for the given transaction key and StarkEx instance ID.
    /// </summary>
    /// <param name="transactionKey">The unique key identifying the transaction.</param>
    /// <param name="starkExInstanceId">The unique ID of the StarkEx instance for which the transaction ID is being allocated.</param>
    /// <param name="cancellationToken">Token used for co-op cancellation.</param>
    /// <returns>A task that returns the allocated transaction ID as a long value.</returns>
    Task<long> AllocateTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId, CancellationToken cancellationToken);

    /// <summary>
    /// Frees the transaction ID previously allocated for the given transaction key and StarkEx instance ID.
    /// </summary>
    /// <param name="transactionKey">The unique key identifying the transaction for which the ID was allocated.</param>
    /// <param name="starkExInstanceId">The unique id of the StarkEx instance for which the transaction ID was allocated.</param>
    /// <returns>A task without return value.</returns>
    Task FreeTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId);
}
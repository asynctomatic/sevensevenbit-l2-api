namespace SevenSevenBit.Operator.Application.Common.Interfaces;

/// <summary>
/// Interface for a service that can interact with a Redis instance.
/// </summary>
public interface INoSqlService
{
    /// <summary>
    /// Unlocks a transaction id and returns the result.
    /// </summary>
    /// <param name="transactionKey">The unique key identifying the transaction for which the ID was unlocked.</param>
    /// <param name="starkExInstanceId">The unique id of the StarkEx instance for which the transaction ID was unlocked.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task UnlockTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId);

    /// <summary>
    /// Locks a transaction id and returns the result.
    /// </summary>
    /// <param name="transactionKey">The unique key identifying the transaction for which the ID was locked.</param>
    /// <param name="starkExInstanceId">The unique id of the StarkEx instance for which the transaction ID was locked.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation result.</returns>
    Task<long> LockTransactionIdAsync(Guid transactionKey, Guid starkExInstanceId);
}
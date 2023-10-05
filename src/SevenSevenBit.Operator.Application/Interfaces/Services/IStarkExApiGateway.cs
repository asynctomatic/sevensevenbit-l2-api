namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using StarkEx.Client.SDK.Models.Spot.TransactionModels;

/// <summary>
/// A service for sending transaction to the StarkEx API.
/// </summary>
public interface IStarkExApiGateway
{
    /// <summary>
    /// Send the transaction to the StarkEx API.
    /// </summary>
    /// <param name="starkExUri">Base address of the StarkEx API.</param>
    /// <param name="starkExVersion">Version of the StarkEx API.</param>
    /// <param name="transaction">The transaction to send.</param>
    /// <param name="transactionId">The sequence number associated with the transaction.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task without return value.</returns>
    Task SendTransactionAsync(
        Uri starkExUri,
        string starkExVersion,
        TransactionModel transaction,
        long transactionId,
        CancellationToken cancellationToken);

    /// <summary>
    /// Returns the next transaction Id from StarkEx API, only applicable in testing instances.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The next transaction id.</returns>
    Task<int> GetFirstUnusedTxAsync(
        CancellationToken cancellationToken);
}
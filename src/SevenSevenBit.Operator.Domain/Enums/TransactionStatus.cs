namespace SevenSevenBit.Operator.Domain.Enums;

/// <summary>
/// Enum that represents all the possible Transaction status.
/// </summary>
public enum TransactionStatus
{
    /// <summary>Initial transaction status, after it has been created in the DB.</summary>
    Created,

    /// <summary>Initial transaction status, after it has been validated and streamed for the Saga Service.</summary>
    Streamed,

    /// <summary>Transaction has been accepted by the StarkEx Gateway.</summary>
    Pending,

    /// <summary>Stark Proof of the transaction has been submitted on-chain.</summary>
    Confirmed,

    /// <summary>StarkEx has deemed the transaction invalid and prompted for alternative transactions.</summary>
    Reverted,

    /// <summary>Transaction failed to be accepted by the StarkEx Gateway.</summary>
    Failed,
}
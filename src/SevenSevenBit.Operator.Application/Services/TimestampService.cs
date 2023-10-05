namespace SevenSevenBit.Operator.Application.Services;

using SevenSevenBit.Operator.Application.Interfaces.Services;

public class TimestampService : ITimestampService
{
    /// <summary>
    ///     The recommended expiration offset for transaction details requested from the system.
    ///     Set by default to eight days in order to give a 1 day window between tx construction and submission.
    /// </summary>
    private const int TargetExpirationOffset = 8;

    /// <summary>
    ///     The lower bound for expiration timestamps of transactions submitted into the system.
    ///     Set by default to seven days in the future as recommended in the StarkEx documentation.
    ///     <para>
    ///         <a href="https://docs.starkware.co/starkex/con_about_handling_invalid_transactions.html" />
    ///     </para>
    /// </summary>
    private const int LimitExpirationOffset = 7;

    public long GetTargetExpirationTimestamp()
    {
        return DateTimeOffset.UtcNow.AddDays(TargetExpirationOffset).ToUnixTimeSeconds();
    }

    public long GetLimitExpirationTimestamp()
    {
        return DateTimeOffset.UtcNow.AddDays(LimitExpirationOffset).ToUnixTimeSeconds();
    }
}
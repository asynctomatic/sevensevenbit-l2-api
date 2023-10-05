namespace SevenSevenBit.Operator.Application.Interfaces.Services;

public interface ITimestampService
{
    long GetTargetExpirationTimestamp();

    long GetLimitExpirationTimestamp();
}
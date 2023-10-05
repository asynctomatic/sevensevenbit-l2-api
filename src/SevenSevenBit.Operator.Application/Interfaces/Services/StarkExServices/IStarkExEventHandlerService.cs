namespace SevenSevenBit.Operator.Application.Interfaces.Services.StarkExServices;

using Nethereum.Contracts;
using SevenSevenBit.Operator.Application.Blockchain.Events;

public interface IStarkExEventHandlerService
{
    Task HandleLogDepositEventAsync(
        EventLog<LogDepositEvent> log,
        CancellationToken cancellationToken);

    Task HandleLogDepositWithTokenIdEventAsync(
        EventLog<LogDepositWithTokenIdEvent> log,
        CancellationToken cancellationToken);

    void HandleLogDepositCancelEvent(
        EventLog<LogDepositCancelEvent> log);

    void HandleLogDepositCancelReclaimedEvent(
        EventLog<LogDepositCancelReclaimedEvent> log);

    void HandleLogDepositWithTokenIdCancelReclaimedEvent(
        EventLog<LogDepositWithTokenIdCancelReclaimedEvent> log);

    Task HandleLogFullWithdrawalRequestEventAsync(
        EventLog<LogFullWithdrawalRequestEvent> log,
        CancellationToken cancellationToken);
}
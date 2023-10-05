namespace SevenSevenBit.Operator.Application.Common.Interfaces;

public interface IMessageBusService
{
    Task Publish<T>(T message, CancellationToken cancellationToken)
        where T : class;
}
namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Services;

using MassTransit;
using SevenSevenBit.Operator.Application.Common.Interfaces;

public class MessageBusService : IMessageBusService
{
    private readonly IPublishEndpoint publishEndpoint;

    public MessageBusService(
        IPublishEndpoint publishEndpoint)
    {
        this.publishEndpoint = publishEndpoint;
    }

    public async Task Publish<T>(T message, CancellationToken cancellationToken)
        where T : class
    {
        await publishEndpoint.Publish(message, cancellationToken);
    }
}
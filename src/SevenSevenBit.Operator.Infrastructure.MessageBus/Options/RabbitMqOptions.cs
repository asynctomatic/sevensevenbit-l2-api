namespace SevenSevenBit.Operator.Infrastructure.MessageBus.Options;

public class RabbitMqOptions
{
    /// <summary>
    /// Gets or sets the hostname or IP address of the RabbitMq server.
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets the username to use when authenticating with RabbitMq.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets the password to use when authenticating with RabbitMq.
    /// </summary>
    public string Password { get; set; }
}
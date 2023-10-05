namespace SevenSevenBit.Operator.Infrastructure.NoSQL.Options;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Represents configuration options for connecting to a Redis instance.
/// </summary>
[ExcludeFromCodeCoverage]
public class RedisOptions
{
    /// <summary>
    /// The configuration key for accessing Redis options in a configuration file.
    /// </summary>
    public const string Redis = "Services:Redis";

    /// <summary>
    /// Gets or sets the connection string for connecting to the Redis instance.
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets the Redis database ID to connect to.
    /// </summary>
    public int DatabaseId { get; set; }

    /// <summary>
    /// Gets or sets the Time to Live (TTL) in seconds for Redis keys.
    /// </summary>
    public int TtlInSeconds { get; set; }
}
namespace SevenSevenBit.Operator.TestHelpers.Extensions;

using Microsoft.Extensions.Logging;
using Moq;

/// <summary>
/// Extension methods for verifying log entries in mocked <see cref="ILogger"/> instances.
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Verifies that a log entry with the specified log level and message was written to the given logger.
    /// </summary>
    /// <typeparam name="T">The type of the logger class.</typeparam>
    /// <param name="logger">The mocked logger.</param>
    /// <param name="logLevel">The log level to verify.</param>
    /// <param name="message">The message to verify.</param>
    public static void VerifyLog<T>(
        this Mock<ILogger<T>> logger,
        LogLevel logLevel,
        string message)
    {
        logger.Verify(
            x =>
                x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Verifies that a log entry with the specified log level, message and exception was written to the given logger.
    /// </summary>
    /// <param name="logger">The mocked logger.</param>
    /// <param name="logLevel">The log level to verify.</param>
    /// <param name="message">The message to verify.</param>
    /// <param name="exception">The exception to verify.</param>
    public static void VerifyLog(
        this Mock<ILogger> logger,
        LogLevel logLevel,
        string message,
        Exception exception)
    {
        logger.Verify(
            x =>
                x.Log(
                    logLevel,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => string.Equals(message, o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}
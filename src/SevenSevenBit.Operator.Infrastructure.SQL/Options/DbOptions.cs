namespace SevenSevenBit.Operator.Infrastructure.SQL.Options;

public class DbOptions
{
    public string DefaultConnectionString { get; set; }

    public int CommandTimeoutInSeconds { get; set; }
}
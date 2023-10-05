namespace SevenSevenBit.Operator.Infrastructure.SQL.Options;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class PostgresOptions
{
    public const string Postgres = "Services:Postgres";

    public DbOptions OperatorDb { get; set; }

    public DbOptions BlockchainDb { get; set; }
}
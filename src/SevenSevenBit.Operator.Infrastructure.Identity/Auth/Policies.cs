namespace SevenSevenBit.Operator.Infrastructure.Identity.Auth;

using System.Diagnostics.CodeAnalysis;
using SevenSevenBit.Operator.Domain.Constants.Auth;

[ExcludeFromCodeCoverage]
public static class Policies
{
    // ADMIN POLICIES
    // TODO: Remove this policy after deprecating the admin endpoints.

    // API POLICIES
    public const string ReadUsers = "ReadUsers";

    public const string WriteUsers = "WriteUsers";

    public const string ReadAssets = "ReadAssets";

    public const string WriteAssets = "WriteAssets";

    public const string MintAssets = "MintAssets";

    public const string ReadVaults = "ReadVaults";

    public const string WriteVaults = "WriteVaults";

    public const string ReadWriteVaults = "ReadWriteVaults";

    public const string WriteTransfers = "WriteTransfers";

    public const string WriteSettlements = "WriteSettlements";

    public const string ReadTransactions = "ReadTransactions";

    public const string WriteMarketplaces = "WriteMarketplaces";

    public const string ReadMarketplaces = "ReadMarketplaces";

    public const string WriteOrders = "WriteOrders";

    public const string ReadOrders = "ReadOrders";

    // StarkEx POLICIES
    public const string AlternativeTxs = "AlternativeTransactions";

    private static readonly IDictionary<string, string[]> ScopesPerPolicy = new Dictionary<string, string[]>
    {
        { ReadUsers, new[] { Scopes.ReadUsers } },
        { WriteUsers, new[] { Scopes.WriteUsers } },
        { WriteAssets, new[] { Scopes.WriteAssets } },
        { ReadAssets, new[] { Scopes.ReadAssets } },
        { MintAssets, new[] { Scopes.MintAssets } },
        { ReadVaults, new[] { Scopes.ReadVaults } },
        { WriteVaults, new[] { Scopes.WriteVaults } },
        { ReadWriteVaults, new[] { Scopes.ReadVaults, Scopes.WriteVaults } },
        { WriteTransfers, new[] { Scopes.WriteTransfers } },
        { WriteSettlements, new[] { Scopes.WriteSettlements } },
        { ReadTransactions, new[] { Scopes.ReadTransactions } },
        { WriteMarketplaces, new[] { Scopes.WriteMarketplaces } },
        { ReadMarketplaces, new[] { Scopes.ReadMarketplaces } },
        { WriteOrders, new[] { Scopes.WriteOrders } },
        { ReadOrders, new[] { Scopes.ReadOrders } },
        { AlternativeTxs, new[] { Scopes.AltTxs } },
    };

    public static string[] GetScopes(this string policy)
    {
        return ScopesPerPolicy[policy];
    }
}
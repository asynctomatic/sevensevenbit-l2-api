namespace SevenSevenBit.Operator.Domain.Constants.Auth;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public static class Scopes
{
    // ADMIN SCOPES
    // TODO: Remove this scope after deprecating the admin endpoints

    // API SCOPES
    public const string WriteUsers = "write:users";

    public const string ReadUsers = "read:users";

    public const string WriteAssets = "write:assets";

    public const string ReadAssets = "read:assets";

    public const string MintAssets = "mint:assets";

    public const string WriteVaults = "write:vaults";

    public const string ReadVaults = "read:vaults";

    public const string WriteTransfers = "write:transfers";

    public const string WriteSettlements = "write:settlements";

    public const string ReadTransactions = "read:transactions";

    public const string WriteMarketplaces = "write:marketplaces";

    public const string ReadMarketplaces = "read:marketplaces";

    public const string WriteOrders = "write:orders";

    public const string ReadOrders = "read:orders";

    // STARKEX SCOPES
    public const string AltTxs = "alt_txs";
}
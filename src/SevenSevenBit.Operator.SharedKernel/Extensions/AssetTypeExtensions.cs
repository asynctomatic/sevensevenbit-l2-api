namespace SevenSevenBit.Operator.SharedKernel.Extensions;

using StarkEx.Crypto.SDK.Enums;

public static class AssetTypeExtensions
{
    public static bool IsAToken(this AssetType assetType)
    {
        return assetType is not AssetType.Eth;
    }

    public static bool IsFungible(this AssetType assetType)
    {
        return assetType is AssetType.Eth or AssetType.Erc20;
    }

    public static bool HasTokenId(this AssetType assetType)
    {
        return assetType is AssetType.Erc721 or AssetType.Erc1155;
    }

    public static bool IsMintable(this AssetType assetType)
    {
        return assetType is AssetType.MintableErc20 or AssetType.MintableErc721 or AssetType.MintableErc1155;
    }

    public static bool IsNft(this AssetType assetType)
    {
        return assetType is AssetType.Erc721 or AssetType.MintableErc721;
    }

    public static bool IsSft(this AssetType assetType)
    {
        return assetType is AssetType.Erc1155 or AssetType.MintableErc1155;
    }

    public static string GetDepositFunctionName(this AssetType assetType)
    {
        return assetType switch
        {
            AssetType.Eth => "depositEth",
            AssetType.Erc20 => "depositERC20",
            AssetType.Erc721 => "depositNft",
            AssetType.Erc1155 => "depositERC1155",
            _ => throw new ArgumentOutOfRangeException(nameof(assetType), assetType, null),
        };
    }

    public static string GetWithdrawFunctionName(this AssetType assetType)
    {
        // TODO confirm the MintableErc1155 withdraw function name
        return assetType switch
        {
            AssetType.Eth => "withdraw",
            AssetType.Erc20 => "withdraw",
            AssetType.Erc721 => "withdrawWithTokenId",
            AssetType.Erc1155 => "withdrawWithTokenId",
            AssetType.MintableErc20 => "withdrawAndMint",
            AssetType.MintableErc721 => "withdrawAndMint",
            AssetType.MintableErc1155 => "withdrawAndMint",
            _ => throw new ArgumentOutOfRangeException(nameof(assetType), assetType, null),
        };
    }
}
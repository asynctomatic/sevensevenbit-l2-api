namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Mint.SubmitSingleMint.Data;

using System.Collections;
using System.Numerics;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api.Mint;

public class SuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Erc20 mint with amount equal to 1 (quantized).
        yield return new object[]
        {
            new BatchMintRequestModel
            {
                Mints = new[]
                {
                    new MintRequestModel
                    {
                        UserId = Users.Alice.Id,
                        AssetId = Assets.MintableErc20.Id,
                        Amount = Assets.MintableErc20.Quantum.Value,
                    },
                },
            },
        };

        // Erc20 mint with amount larger than 1 (quantized).
        yield return new object[]
        {
            new BatchMintRequestModel
            {
                Mints = new[]
                {
                    new MintRequestModel
                    {
                        UserId = Users.Alice.Id,
                        AssetId = Assets.MintableErc20.Id,
                        Amount = BigInteger.Parse("1000") * Assets.MintableErc20.Quantum.Value,
                    },
                },
            },
        };

        // Erc721 mint with amount equal to 1 (quantized).
        yield return new object[]
        {
            new BatchMintRequestModel
            {
                Mints = new[]
                {
                    new MintRequestModel
                    {
                        UserId = Users.Alice.Id,
                        AssetId = Assets.MintableErc721.Id,
                        Amount = BigInteger.One,
                    },
                },
            },
        };

        // Erc1155 mint with amount equal to 1 (quantized).
        yield return new object[]
        {
            new BatchMintRequestModel
            {
                Mints = new[]
                {
                    new MintRequestModel
                    {
                        UserId = Users.Alice.Id,
                        AssetId = Assets.MintableErc1155.Id,
                        Amount = Assets.MintableErc1155.Quantum.Value,
                    },
                },
            },
        };

        // Erc1155 mint with amount larger than 1 (quantized).
        yield return new object[]
        {
            new BatchMintRequestModel
            {
                Mints = new[]
                {
                    new MintRequestModel
                    {
                        UserId = Users.Alice.Id,
                        AssetId = Assets.MintableErc1155.Id,
                        Amount = BigInteger.Parse("1000") * Assets.MintableErc1155.Quantum.Value,
                    },
                },
            },
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
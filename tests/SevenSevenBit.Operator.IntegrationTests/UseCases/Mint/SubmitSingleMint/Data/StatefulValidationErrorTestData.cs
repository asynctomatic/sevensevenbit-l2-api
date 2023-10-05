namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Mint.SubmitSingleMint.Data;

using System.Collections;
using System.Net;
using System.Numerics;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.TestHelpers.Data;
using SevenSevenBit.Operator.Web.Models.Api.Mint;

public class StatefulValidationErrorTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Mint of non-mintable asset.
        yield return new object[]
        {
            new BatchMintRequestModel
            {
                Mints = new[]
                {
                    new MintRequestModel
                    {
                        UserId = Users.Alice.Id,
                        AssetId = Assets.Eth.Id,
                        Amount = Assets.Eth.Quantum.Value,
                    },
                },
            },
            HttpStatusCode.BadRequest,
            ((int)ErrorCodes.InvalidAssetType).ToString(),
        };

        // Erc20 mint with unquantized amount.
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
                        Amount = Assets.MintableErc20.Quantum.Value - BigInteger.One,
                    },
                },
            },
            HttpStatusCode.BadRequest,
            ((int)ErrorCodes.OperationAmountUnquantizable).ToString(),
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
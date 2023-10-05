namespace SevenSevenBit.Operator.IntegrationTests.UseCases.User.RegisterUser.Data;

using System.Collections;
using System.Net;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Models.Api;

public class StatefulValidationErrorTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // Duplicate registration.
        yield return new object[]
        {
            new RegisterUserModel
            {
                StarkKey = "0x0200499f65ae2f71d5298d2d88823b2e5e19596a71aac1984710479e406a002439",
                StarkSignature = new SignatureModel
                {
                    R = "0x68b1d51ccf5d49ec334c7c2111855f0cd8bffbb7f4db2c820f409ed227877d4",
                    S = "0x1614058324bb03628f7b2aa368279546b2d7b1c3aba94ac2b66dcedc6762862",
                },
            },
            HttpStatusCode.BadRequest,
            ((int)ErrorCodes.StarkKeyAlreadyInUse).ToString(),
        };

        // Invalid registration signature.
        yield return new object[]
        {
            new RegisterUserModel
            {
                StarkKey = "0x0200499f65ae2f71d5298d2d88823b2e5e19596a71aac1984710479e406a002439",
                StarkSignature = new SignatureModel
                {
                    R = "0x68b1d51ccf5d49ec334c",
                    S = "0x1614058324bb03628f7b",
                },
            },
            HttpStatusCode.BadRequest,
            ((int)ErrorCodes.ModelStateInvalid).ToString(),
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
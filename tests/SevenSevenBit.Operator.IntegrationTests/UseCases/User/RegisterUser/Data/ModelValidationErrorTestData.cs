namespace SevenSevenBit.Operator.IntegrationTests.UseCases.User.RegisterUser.Data;

using System.Collections;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Web.Models.Api;

public class ModelValidationErrorTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            new RegisterUserModel
            {
                StarkKey = string.Empty,
                StarkSignature = new SignatureModel
                {
                    R = string.Empty,
                    S = string.Empty,
                },
            },
            ((int)ErrorCodes.ModelStateInvalid).ToString(), // TODO: The 1000 code is the same everywhere...
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
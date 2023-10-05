namespace SevenSevenBit.Operator.IntegrationTests.UseCases.User.RegisterUser.Data;

using System.Collections;
using SevenSevenBit.Operator.Web.Models.Api;

public class SuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        yield return new object[]
        {
            // Valid model with 0x prefixed hex strings.
            new RegisterUserModel
            {
                // Private Key: 0x544795805cd03e9ee7b150d9ab791af43a07f26da485431fd4a471efbee0da1
                StarkKey = "2fa041f5f65e13adfa5ab32556b307ee8b20c3c885b2919dd74c045e1df6b31",
                StarkSignature = new SignatureModel
                {
                    R = "0x5897b634c734c31525c83d7bd7e5e012c03e54b2092131648dfb088fc4a7323",
                    S = "0xf43ea0f539a81427b4ebe8a0dccfb264824b76d3e5833443e26ca7a59f0f09",
                },
            },
        };
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
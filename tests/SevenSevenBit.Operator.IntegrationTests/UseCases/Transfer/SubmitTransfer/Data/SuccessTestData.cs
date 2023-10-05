namespace SevenSevenBit.Operator.IntegrationTests.UseCases.Transfer.SubmitTransfer.Data;

using System.Collections;
using SevenSevenBit.Operator.TestHelpers.Data;

public class SuccessTestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        // TODO: There is a bug somewhere in the code that causes the following test to fail.
        yield return new object[]
        {
            Users.Alice,
            Users.Bob,
            Assets.Eth,
            "1000",
            "100",
        };

        // TODO: token Id...
        yield return new object[]
        {
            Users.Alice,
            Users.Carol,
            Assets.MiladyErc721,
            "1",
            "1",
        };
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
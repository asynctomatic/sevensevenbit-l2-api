namespace SevenSevenBit.Operator.IntegrationTests.Fixture;

using SevenSevenBit.Operator.IntegrationTests.Helpers;
using WireMock.Matchers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

public class StarkExFixture : IDisposable
{
    private readonly WireMockServer wireMockServer;
    private bool disposed;

    public StarkExFixture()
    {
        // Turn off logging in integration tests as it makes it harder to debug integration tests in pipeline logs
        wireMockServer = WiremockHelper.StartWiremockServer(8080);

        // Match next transaction ID request to the StarkEx gateway API with incrementing ID values
        wireMockServer
            .Given(Request.Create()
                .WithPath("/v2/gateway/testing/get_first_unused_tx_id").UsingGet())
            .InScenario("tx_id state")
            .WillSetStateTo("tx_id:1")
            .RespondWith(Response.Create()
                .WithBody("0"));
        foreach (var txId in Enumerable.Range(1, 100))
        {
            wireMockServer
                .Given(Request.Create()
                    .WithPath("/v2/gateway/testing/get_first_unused_tx_id").UsingGet())
                .InScenario("tx_id state")
                .WhenStateIs($"tx_id:{txId}")
                .WillSetStateTo($"tx_id:{txId + 1}")
                .RespondWith(Response.Create()
                    .WithBody(txId.ToString()));
        }

        // Match any tx submission request to the StarkEx gateway API with a 'TRANSACTION_PENDING' response
        wireMockServer
            .Given(Request.Create()
                .WithPath("/v2/gateway/add_transaction").UsingPost()
                .WithBody(new NotNullOrEmptyMatcher()))
            .RespondWith(Response.Create()
                .WithBody(@"{""code"": ""TRANSACTION_PENDING""}"));
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed && disposing)
        {
            wireMockServer?.Dispose();
        }

        disposed = true;
    }
}
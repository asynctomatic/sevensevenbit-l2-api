namespace SevenSevenBit.Operator.Infrastructure.StarkExApi.Factories;

using SevenSevenBit.Operator.Application.Common.Interfaces;
using StarkEx.Client.SDK.Clients.Spot;
using StarkEx.Client.SDK.Interfaces.Spot;
using StarkEx.Client.SDK.Settings;

public class SpotGatewayClientFactory : IFactory<StarkExApiSettings, ISpotGatewayClient>
{
    private readonly IHttpClientFactory httpClientFactory;

    public SpotGatewayClientFactory(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
        httpClientFactory.CreateClient();
    }

    public ISpotGatewayClient Create(StarkExApiSettings input)
    {
        return new SpotGatewayClient(httpClientFactory, input);
    }
}
namespace SevenSevenBit.Operator.IntegrationTests.Collections;

using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.UseCases;
using Xunit;

[CollectionDefinition("Api Integration Tests")]
public class ApiTestCollection : ICollectionFixture<OperatorApiFactory>, ICollectionFixture<DatabaseFixture>
{
}
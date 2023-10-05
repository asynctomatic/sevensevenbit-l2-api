namespace SevenSevenBit.Operator.IntegrationTests.Collections;

using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.Fixture.Blockchain;
using SevenSevenBit.Operator.IntegrationTests.UseCases;
using Xunit;

[CollectionDefinition("Worker Integration Tests", DisableParallelization = true)]
public class WorkerTestCollection :
    ICollectionFixture<OperatorWorkerFactory>,
    ICollectionFixture<DatabaseFixture>,
    ICollectionFixture<BlockchainFixture>
{
}
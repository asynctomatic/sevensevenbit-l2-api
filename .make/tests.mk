# tests.mk
# Contains rules for running unit and integration tests.

# Path to the unit test project
UNIT_TEST_PROJECT := ./tests/SevenSevenBit.Operator.UnitTests/SevenSevenBit.Operator.UnitTests.csproj
# Path to the integration test project
INTEGRATION_TEST_PROJECT := ./tests/SevenSevenBit.Operator.IntegrationTests/SevenSevenBit.Operator.IntegrationTests.csproj
# Desired environment setting for the integration tests
ENVIRONMENT := IntegrationTests

.PHONY: test
test: unit-test integration-test

.PHONY: unit-test
# Runs unit tests on the specified project using dotnet test.
# Optionally, a specific tag can be provided to run a subset of tests. 
# Usage: make unit-test tag=<your-tag>
unit-test:
	@dotnet test $(if $(tag),--filter Type=${tag},) $(UNIT_TEST_PROJECT)

.PHONY: integration-test
# Runs integration tests on the specified project using dotnet test.
# The ASPNETCORE_ENVIRONMENT variable is set to 'IntegrationTests' for the duration of the command.
# Optionally, a specific tag can be provided to run a subset of tests. 
# Usage: make integration-test tag=<your-tag>
integration-test:
	ASPNETCORE_ENVIRONMENT=$(ENVIRONMENT) dotnet test \
		$(if $(tag),--filter Type=${tag},) $(INTEGRATION_TEST_PROJECT)

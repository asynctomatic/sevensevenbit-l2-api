# SDK
ARG DOTNET_SDK_VERSION=6.0-alpine

# RUN TIME
ARG DOTNET_RUNTIME_VERSION=6.0-alpine

# REPO
ARG FROM_REPO_INFRASTRUCTURE_DOTNET_SDK=mcr.microsoft.com/dotnet/sdk
ARG FROM_REPO_INFRASTRUCTURE_DOTNET_RUNTIME=mcr.microsoft.com/dotnet/runtime
ARG FROM_REPO_INFRASTRUCTURE_DOTNET_ASPNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet

FROM ${FROM_REPO_INFRASTRUCTURE_DOTNET_SDK}:${DOTNET_SDK_VERSION} AS build

ARG RELEASE_VERSION

WORKDIR /home/app

#1 - Copying the .sln and .csproj files improves build performance
COPY SevenSevenBit.Operator.sln SevenSevenBit.Operator.sln

COPY src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj
COPY src/SevenSevenBit.Operator.Worker/SevenSevenBit.Operator.Worker.csproj src/SevenSevenBit.Operator.Worker/SevenSevenBit.Operator.Worker.csproj

COPY src/SevenSevenBit.Operator.Application/SevenSevenBit.Operator.Application.csproj src/SevenSevenBit.Operator.Application/SevenSevenBit.Operator.Application.csproj
COPY src/SevenSevenBit.Operator.Domain/SevenSevenBit.Operator.Domain.csproj src/SevenSevenBit.Operator.Domain/SevenSevenBit.Operator.Domain.csproj
COPY src/SevenSevenBit.Operator.SharedKernel/SevenSevenBit.Operator.SharedKernel.csproj src/SevenSevenBit.Operator.SharedKernel/SevenSevenBit.Operator.SharedKernel.csproj
COPY src/SevenSevenBit.Operator.Infrastructure.Blockchain/SevenSevenBit.Operator.Infrastructure.Blockchain.csproj src/SevenSevenBit.Operator.Infrastructure.Blockchain/SevenSevenBit.Operator.Infrastructure.Blockchain.csproj
COPY src/SevenSevenBit.Operator.Infrastructure.Identity/SevenSevenBit.Operator.Infrastructure.Identity.csproj src/SevenSevenBit.Operator.Infrastructure.Identity/SevenSevenBit.Operator.Infrastructure.Identity.csproj
COPY src/SevenSevenBit.Operator.Infrastructure.MessageBus/SevenSevenBit.Operator.Infrastructure.MessageBus.csproj src/SevenSevenBit.Operator.Infrastructure.MessageBus/SevenSevenBit.Operator.Infrastructure.MessageBus.csproj
COPY src/SevenSevenBit.Operator.Infrastructure.NoSQL/SevenSevenBit.Operator.Infrastructure.NoSQL.csproj src/SevenSevenBit.Operator.Infrastructure.NoSQL/SevenSevenBit.Operator.Infrastructure.NoSQL.csproj
COPY src/SevenSevenBit.Operator.Infrastructure.SQL/SevenSevenBit.Operator.Infrastructure.SQL.csproj src/SevenSevenBit.Operator.Infrastructure.SQL/SevenSevenBit.Operator.Infrastructure.SQL.csproj
COPY src/SevenSevenBit.Operator.Infrastructure.StarkExApi/SevenSevenBit.Operator.Infrastructure.StarkExApi.csproj src/SevenSevenBit.Operator.Infrastructure.StarkExApi/SevenSevenBit.Operator.Infrastructure.StarkExApi.csproj

COPY tests/SevenSevenBit.Operator.TestHelpers/SevenSevenBit.Operator.TestHelpers.csproj tests/SevenSevenBit.Operator.TestHelpers/SevenSevenBit.Operator.TestHelpers.csproj
COPY tests/SevenSevenBit.Operator.IntegrationTests/SevenSevenBit.Operator.IntegrationTests.csproj tests/SevenSevenBit.Operator.IntegrationTests/SevenSevenBit.Operator.IntegrationTests.csproj

COPY src/SevenSevenBit.Operator.SagaService/SevenSevenBit.Operator.SagaService.csproj src/SevenSevenBit.Operator.SagaService/SevenSevenBit.Operator.SagaService.csproj

COPY src/SevenSevenBit.Operator.TransactionIdService/SevenSevenBit.Operator.TransactionIdService.csproj src/SevenSevenBit.Operator.TransactionIdService/SevenSevenBit.Operator.TransactionIdService.csproj

COPY src/SevenSevenBit.Operator.TransactionStreamService/SevenSevenBit.Operator.TransactionStreamService.csproj src/SevenSevenBit.Operator.TransactionStreamService/SevenSevenBit.Operator.TransactionStreamService.csproj

COPY Directory.Packages.props Directory.Packages.props
COPY nuget.config nuget.config
COPY sevensevenbit.operator.ruleset sevensevenbit.operator.ruleset

# 1.1 - Restore nugets
RUN dotnet restore SevenSevenBit.Operator.sln --ignore-failed-sources --configfile nuget.config

#2 - copy all files (from the "context" defined in docker-compose)
COPY . .

#3 - Publish 
RUN dotnet publish src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj -c Release -o /home/out/operator -p:Version=${RELEASE_VERSION} --no-restore
RUN dotnet publish src/SevenSevenBit.Operator.Worker/SevenSevenBit.Operator.Worker.csproj -c Release -o /home/out/worker -p:Version=${RELEASE_VERSION} --no-restore

RUN dotnet publish src/SevenSevenBit.Operator.SagaService/SevenSevenBit.Operator.SagaService.csproj -c Release -o /home/out/saga-service -p:Version=${RELEASE_VERSION} --no-restore
RUN dotnet publish src/SevenSevenBit.Operator.TransactionIdService/SevenSevenBit.Operator.TransactionIdService.csproj -c Release -o /home/out/transaction-id-service -p:Version=${RELEASE_VERSION} --no-restore
RUN dotnet publish src/SevenSevenBit.Operator.TransactionStreamService/SevenSevenBit.Operator.TransactionStreamService.csproj -c Release -o /home/out/transaction-stream-service -p:Version=${RELEASE_VERSION} --no-restore

#4 - Bundle Migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

RUN dotnet ef migrations script --idempotent \
    --project ./src/SevenSevenBit.Operator.Infrastructure.SQL/SevenSevenBit.Operator.Infrastructure.SQL.csproj \
    --startup-project ./src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj \
    --output /home/out/operator/migrations/operator.sql \
    --context OperatorDbContext 

RUN dotnet ef migrations script --idempotent \
    --project ./src/SevenSevenBit.Operator.Infrastructure.SQL/SevenSevenBit.Operator.Infrastructure.SQL.csproj \
    --startup-project ./src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj \
    --output /home/out/blockchain/migrations/blockchain.sql \
    --context BlockchainDbContext

#6.1 - Operator Integration Tests
FROM build AS integration-tests

WORKDIR /home/app

ENV ASPNETCORE_ENVIRONMENT=IntegrationTests

CMD dotnet build -c Release && \
    dotnet test tests/SevenSevenBit.Operator.IntegrationTests/SevenSevenBit.Operator.IntegrationTests.csproj --no-build -c Release --logger "trx" \
    --results-directory /home/reports /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

#7 - Create new image with a single layer from runtime version
FROM ${FROM_REPO_INFRASTRUCTURE_DOTNET_RUNTIME}:${DOTNET_RUNTIME_VERSION} as base-runtime

WORKDIR /home/app

# New Relic configuration
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A}
ENV CORECLR_PROFILER_PATH=/home/app/newrelic/libNewRelicProfiler.so
ENV CORECLR_NEWRELIC_HOME=/home/app/newrelic

ENV DOTNET_ENVIRONMENT=Production

FROM ${FROM_REPO_INFRASTRUCTURE_DOTNET_ASPNET_RUNTIME}:${DOTNET_RUNTIME_VERSION} As base-asp-runtime

WORKDIR /home/app

# New Relic configuration
ENV CORECLR_ENABLE_PROFILING=1
ENV CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A}
ENV CORECLR_PROFILER_PATH=/home/app/newrelic/libNewRelicProfiler.so
ENV CORECLR_NEWRELIC_HOME=/home/app/newrelic

ENV ASPNETCORE_URLS=http://+:9179
ENV ASPNETCORE_ENVIRONMENT=Production

FROM base-asp-runtime AS operator-runtime

RUN mkdir bin

COPY --from=build /home/out/operator .
RUN mv /home/app/newrelic.config /home/app/newrelic/newrelic.config

RUN apk update && apk add ca-certificates --no-cache && rm -rf /var/cache/apk/*
COPY ./certificates /usr/local/share/ca-certificates/
RUN update-ca-certificates

ENV NEW_RELIC_APP_NAME=sevensevenbit-operator-local

CMD ["dotnet", "SevenSevenBit.Operator.Web.dll"]

FROM base-asp-runtime AS worker-runtime

RUN mkdir bin

COPY --from=build /home/out/worker .
RUN mv /home/app/newrelic.config /home/app/newrelic/newrelic.config

ENV NEW_RELIC_APP_NAME=sevensevenbit-operator-worker-local

ENTRYPOINT ["dotnet", "SevenSevenBit.Operator.Worker.dll"]

FROM base-asp-runtime As saga-service-runtime

RUN mkdir bin

COPY --from=build /home/out/saga-service .
RUN mv /home/app/newrelic.config /home/app/newrelic/newrelic.config

ENV NEW_RELIC_APP_NAME=sevensevenbit-operator-saga-local

ENTRYPOINT ["dotnet", "SevenSevenBit.Operator.SagaService.dll"]

FROM base-asp-runtime As transaction-id-service-runtime

RUN mkdir bin

COPY --from=build /home/out/transaction-id-service .
RUN mv /home/app/newrelic.config /home/app/newrelic/newrelic.config

ENV NEW_RELIC_APP_NAME=sevensevenbit-operator-txid-local

ENTRYPOINT ["dotnet", "SevenSevenBit.Operator.TransactionIdService.dll"]

FROM base-asp-runtime As transaction-stream-service-runtime

RUN mkdir bin

COPY --from=build /home/out/transaction-stream-service .
RUN mv /home/app/newrelic.config /home/app/newrelic/newrelic.config

RUN apk update && apk add ca-certificates && rm -rf /var/cache/apk/*
COPY ./certificates /usr/local/share/ca-certificates/
RUN update-ca-certificates

ENV NEW_RELIC_APP_NAME=sevensevenbit-operator-txstream-local

ENTRYPOINT ["dotnet", "SevenSevenBit.Operator.TransactionStreamService.dll"]

FROM alpine As operator-migrations

RUN apk --no-cache add postgresql14-client bash

ENV PGHOST=postgres-sevensevenbit-operator
ENV PGPORT=5432
ENV PGUSER=postgres
ENV PGPASSWORD=somepassword
ENV PGDATABASE=postgres
ENV DATABASE_NAME=sevensevenbit-operator
ENV SQL_SCRIPT_NAME=operator.sql

WORKDIR /home/app

COPY --from=build /home/out/operator/migrations/operator.sql .
COPY apply_migrations.sh apply_migrations.sh

RUN chmod +x apply_migrations.sh

ENTRYPOINT [ "bash", "-c", "./apply_migrations.sh"]

FROM alpine As blockchain-migrations

RUN apk --no-cache add postgresql14-client bash

ENV PGHOST=postgres-sevensevenbit-operator
ENV PGPORT=5432
ENV PGUSER=postgres
ENV PGPASSWORD=somepassword
ENV PGDATABASE=postgres
ENV DATABASE_NAME=sevensevenbit-operator-blockchain
ENV SQL_SCRIPT_NAME=blockchain.sql

WORKDIR /home/app

COPY --from=build /home/out/blockchain/migrations/blockchain.sql .
COPY apply_migrations.sh apply_migrations.sh

RUN chmod +x apply_migrations.sh

ENTRYPOINT [ "bash", "-c", "./apply_migrations.sh"]
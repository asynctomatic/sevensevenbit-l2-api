ARG DOTNET_SDK_VERSION=6.0-alpine

# Stage 0:
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION AS setup
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.2

WORKDIR /app
COPY . .
RUN dotnet subset restore src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj --root-directory /app --output restore_subset/

# Stage 1: 
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_SDK_VERSION} AS build
WORKDIR /app

# Restore NuGet packages.
COPY --from=setup /app/restore_subset .
RUN dotnet restore src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj

COPY sevensevenbit.operator.ruleset .
COPY src/SevenSevenBit.Operator.Web ./src/SevenSevenBit.Operator.Web
COPY src/SevenSevenBit.Operator.Infrastructure.Blockchain ./src/SevenSevenBit.Operator.Infrastructure.Blockchain
COPY src/SevenSevenBit.Operator.Infrastructure.Identity ./src/SevenSevenBit.Operator.Infrastructure.Identity
COPY src/SevenSevenBit.Operator.Infrastructure.MessageBus ./src/SevenSevenBit.Operator.Infrastructure.MessageBus
COPY src/SevenSevenBit.Operator.Infrastructure.NoSQL ./src/SevenSevenBit.Operator.Infrastructure.NoSQL
COPY src/SevenSevenBit.Operator.Infrastructure.SQL ./src/SevenSevenBit.Operator.Infrastructure.SQL
COPY src/SevenSevenBit.Operator.Infrastructure.StarkExApi ./src/SevenSevenBit.Operator.Infrastructure.StarkExApi
COPY src/SevenSevenBit.Operator.Application ./src/SevenSevenBit.Operator.Application
COPY src/SevenSevenBit.Operator.Domain ./src/SevenSevenBit.Operator.Domain
COPY src/SevenSevenBit.Operator.SharedKernel ./src/SevenSevenBit.Operator.SharedKernel

# Build the application.
RUN dotnet build ./src/SevenSevenBit.Operator.Web --no-restore

ENV PATH="$PATH:/root/.dotnet/tools"
RUN dotnet tool install --global dotnet-ef
RUN dotnet ef migrations script --no-build --idempotent \
    --project ./src/SevenSevenBit.Operator.Infrastructure.SQL/SevenSevenBit.Operator.Infrastructure.SQL.csproj \
    --startup-project ./src/SevenSevenBit.Operator.Web/SevenSevenBit.Operator.Web.csproj \
    --output /home/out/blockchain/migrations/blockchain.sql \
    --context BlockchainDbContext 

# Stage 2:
FROM alpine:latest AS migrations

RUN apk --no-cache add postgresql14-client bash

ENV PGHOST=db
ENV PGPORT=5432
ENV PGUSER=postgres
ENV PGPASSWORD=somepassword
ENV PGDATABASE=postgres
ENV DATABASE_NAME=sevensevenbit-blockchain-db
ENV SQL_SCRIPT_NAME=blockchain.sql

WORKDIR /home/app

COPY --from=build /home/out/blockchain/migrations/blockchain.sql .
COPY apply_migrations.sh apply_migrations.sh

RUN chmod +x apply_migrations.sh

ENTRYPOINT [ "bash", "-c", "./apply_migrations.sh"]
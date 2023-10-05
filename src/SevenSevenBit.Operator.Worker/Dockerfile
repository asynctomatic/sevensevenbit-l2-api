ARG DOTNET_RUNTIME_VERSION=6.0-alpine
ARG DOTNET_SDK_VERSION=6.0-alpine

# Stage 0:
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION AS prepare-restore-files
ENV PATH="${PATH}:/root/.dotnet/tools"
RUN dotnet tool install --global --no-cache dotnet-subset --version 0.3.2

WORKDIR /app
COPY . .
RUN dotnet subset restore src/SevenSevenBit.Operator.Worker/SevenSevenBit.Operator.Worker.csproj --root-directory /app --output restore_subset/

# Stage 1: Build the application.
FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_SDK_VERSION AS build
WORKDIR /app

# Restore NuGet packages.
COPY --from=prepare-restore-files /app/restore_subset .
RUN dotnet restore src/SevenSevenBit.Operator.Worker/SevenSevenBit.Operator.Worker.csproj

COPY sevensevenbit.operator.ruleset .
COPY src/SevenSevenBit.Operator.Worker ./src/SevenSevenBit.Operator.Worker
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
WORKDIR /app/src/SevenSevenBit.Operator.Worker
RUN dotnet build -c Release --no-restore

# Stage 2: Publish the application.
FROM build AS publish
WORKDIR /app/src/SevenSevenBit.Operator.Worker
RUN dotnet publish -c Release --no-restore --no-build -o /app/publish

# Stage 3: Create the runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_RUNTIME_VERSION AS runtime
WORKDIR /app

# Copy the published artifacts to the runtime image.
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Run the application.
ENTRYPOINT ["dotnet", "SevenSevenBit.Operator.Worker.dll"]
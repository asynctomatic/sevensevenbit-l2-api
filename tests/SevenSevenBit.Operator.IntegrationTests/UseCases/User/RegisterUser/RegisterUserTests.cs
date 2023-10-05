namespace SevenSevenBit.Operator.IntegrationTests.UseCases.User.RegisterUser;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.IntegrationTests.UseCases.User.RegisterUser.Data;
using SevenSevenBit.Operator.Web.Models.Api;
using StarkEx.Crypto.SDK.Hashing;
using StarkEx.Crypto.SDK.Signing;
using Xunit;

[Trait("Type", "User")]
[Trait("UseCase", "RegisterUser")]
[Collection("Api Integration Tests")]
public class RegisterUserTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public RegisterUserTests(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    [Theory]
    [ClassData(typeof(ModelValidationErrorTestData))]
    public async Task RegisterUser_ModelValidationError(
        RegisterUserModel registerUserModel, string errorCode)
    {
        // Arrange
        var client = api.HttpClient;

        // Act
        // Send user registration POST request to /api/users endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("users", UriKind.Relative),
            registerUserModel,
            api.JsonSerializerOptions);

        // Deserialize response body into ProblemDetails object.
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        problemDetails.Type.Should().Be(errorCode);
    }

    [Theory]
    [ClassData(typeof(StatefulValidationErrorTestData))]
    public async Task RegisterUser_StatefulValidationError(
        RegisterUserModel registerUserModel, HttpStatusCode statusCode, string errorCode)
    {
        // Arrange
        var client = api.HttpClient;

        // Act
        // Send user registration POST request to /api/users endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("users", UriKind.Relative),
            registerUserModel,
            api.JsonSerializerOptions);

        // Deserialize response body into ProblemDetails object.
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        // Assert
        response.StatusCode.Should().Be(statusCode);
        problemDetails.Type.Should().Be(errorCode);
    }

    [Theory]
    [ClassData(typeof(SuccessTestData))]
    public async Task RegisterUser_HappyPath_UserRegistered(
        RegisterUserModel registerUserModel)
    {
        // Arrange
        var client = api.HttpClient;

        var starkCurve = new StarkCurve();
        IPedersenHash pedersenHash = new PedersenHash(starkCurve);
        var hash = registerUserModel.ToPedersenHash(pedersenHash);
        IStarkExSigner starkExSigner = new StarkExSigner(starkCurve);
        var signature = starkExSigner.SignMessage(hash, "544795805cd03e9ee7b150d9ab791af43a07f26da485431fd4a471efbee0da1");

        // Act
        // Send user registration POST request to /api/users endpoint.
        var response = await client.PostAsJsonAsync(
            new Uri("users", UriKind.Relative),
            registerUserModel,
            api.JsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Headers.Location.Should().NotBeNull();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => database.ResetAsync();
}
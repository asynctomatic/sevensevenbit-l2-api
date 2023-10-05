namespace SevenSevenBit.Operator.IntegrationTests.UseCases.User.GetUserById;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.TestHelpers.Data;
using Xunit;

[Collection("Api Integration Tests")]
public class GetUserByIdTests : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public GetUserByIdTests(OperatorApiFactory api, DatabaseFixture database)
    {
        this.api = api;
        this.database = database;
    }

    public async Task InitializeAsync()
    {
        // TODO: Move to db fixture class.
        var unitOfWork = new UnitOfWork(database.Context);

        // Seed the database with users.
        await unitOfWork.Repository<User>().InsertAsync(Users.GetUsers(), CancellationToken.None);
        await unitOfWork.SaveAsync(CancellationToken.None);
    }

    public Task DisposeAsync() => database.ResetAsync();

    [Fact]
    public async Task GetUserById_UserExists_UserIsReturned()
    {
        // Arrange
        var userId = Users.Alice.Id;

        // Act
        var response = await api.HttpClient.GetAsync(new Uri($"users/{userId}", UriKind.Relative));
        var user = await response.Content.ReadFromJsonAsync<UserDto>(api.JsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetUserById_UserDoesNotExist_NotFoundIsReturned()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var response = await api.HttpClient.GetAsync(new Uri($"users/{userId}", UriKind.Relative));

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
namespace SevenSevenBit.Operator.IntegrationTests.UseCases.User.GetUsers;

using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.WebUtilities;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;
using SevenSevenBit.Operator.IntegrationTests.Fixture;
using SevenSevenBit.Operator.TestHelpers.Data;
using Xunit;

[Collection("Api Integration Tests")]
public class GetUsersTest : IAsyncLifetime
{
    private readonly OperatorApiFactory api;
    private readonly DatabaseFixture database;

    public GetUsersTest(OperatorApiFactory api, DatabaseFixture database)
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
    public async Task GetUsers_UsersExist_PaginatedUsersAreReturned()
    {
        // Arrange
        const int pageNumber = 1;
        var pageSize = Users.GetUsers().Count() - 1;

        var query = new Dictionary<string, string>
        {
            { "page_number", pageNumber.ToString() },
            { "page_size", pageSize.ToString() },
        };

        // Act
        var response = await api.HttpClient.GetAsync(
            new Uri(QueryHelpers.AddQueryString("users", query), UriKind.Relative));
        var paginatedUsers = await response.Content.ReadFromJsonAsync<PaginatedResponseDto<UserDto>>(
            api.JsonSerializerOptions);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        paginatedUsers.Data.Count.Should().Be(pageSize);
        paginatedUsers.Metadata.HasNext.Should().BeTrue();
        paginatedUsers.Metadata.TotalCount.Should().Be(Users.GetUsers().Count());
    }
}
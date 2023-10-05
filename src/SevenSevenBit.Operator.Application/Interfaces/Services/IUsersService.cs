namespace SevenSevenBit.Operator.Application.Interfaces.Services;

using NodaTime;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;

public interface IUsersService
{
    Task<bool> IsStarkKeyUsedAsync(
        StarkKey starkKey,
        CancellationToken cancellationToken);

    Task<User> RegisterUserAsync(
        StarkKey starkKey,
        CancellationToken cancellationToken);

    Task<User> GetUserAsync(
        Guid userId,
        CancellationToken cancellationToken);

    Task<IEnumerable<User>> GetUsersAsync(
        CancellationToken cancellationToken);

    Task<IEnumerable<User>> GetUsersAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken);

    Task<PaginatedResponseDto<UserDto>> GetUsersAsync(
        Paging paging,
        LocalDateTime? creationDate,
        FilterOptions? creationDateFilterOption,
        string sort = null,
        CancellationToken cancellationToken = default);
}
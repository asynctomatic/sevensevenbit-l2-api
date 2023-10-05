namespace SevenSevenBit.Operator.Application.Services;

using NodaTime;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.DTOs.Entities;
using SevenSevenBit.Operator.Application.DTOs.Pagination;
using SevenSevenBit.Operator.Application.Helpers;
using SevenSevenBit.Operator.Application.Interfaces.Services;
using SevenSevenBit.Operator.Domain.Entities;
using SevenSevenBit.Operator.Domain.Enums;
using SevenSevenBit.Operator.Domain.ValueObjects;

public class UserService : IUsersService
{
    private readonly IUnitOfWork unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<bool> IsStarkKeyUsedAsync(
        StarkKey starkKey,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<User>().AnyAsync(
            filter: user => user.StarkKey == starkKey,
            cancellationToken: cancellationToken);
    }

    public async Task<User> RegisterUserAsync(
        StarkKey starkKey,
        CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            StarkKey = starkKey,
        };

        await unitOfWork.Repository<User>().InsertAsync(newUser, cancellationToken);
        await unitOfWork.SaveAsync(cancellationToken);

        return newUser;
    }

    public async Task<User> GetUserAsync(
        Guid userId,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<User>().GetByIdAsync(userId, cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<User>().GetAsync(cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(
        IEnumerable<Guid> userIds,
        CancellationToken cancellationToken)
    {
        return await unitOfWork.Repository<User>().GetAsync(
            filter: user => userIds.Contains(user.Id),
            cancellationToken: cancellationToken);
    }

    public async Task<PaginatedResponseDto<UserDto>> GetUsersAsync(
        Paging paging,
        LocalDateTime? creationDate,
        FilterOptions? creationDateFilterOption,
        string sort = null,
        CancellationToken cancellationToken = default)
    {
        var filter = QueryBuilder.GetFilter(
            (nameof(User.CreationDate), typeof(LocalDateTime), creationDate, creationDateFilterOption));

        sort ??= $"{nameof(User.CreationDate)} desc";

        return await unitOfWork.Repository<User>().GetProjectedPaginatedAsync(
            paging,
            user => new UserDto(user),
            filter: filter,
            orderBy: sort,
            cancellationToken: cancellationToken);
    }
}
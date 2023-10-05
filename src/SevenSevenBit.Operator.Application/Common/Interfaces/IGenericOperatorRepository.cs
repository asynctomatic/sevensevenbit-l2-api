namespace SevenSevenBit.Operator.Application.Common.Interfaces;

using System.Linq.Expressions;
using System.Numerics;
using SevenSevenBit.Operator.Application.DTOs.Pagination;

public interface IGenericOperatorRepository<TEntity>
    where TEntity : class
{
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default);

    Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        CancellationToken cancellationToken = default);

    Task<PaginatedResponseDto<TProjection>> GetProjectedPaginatedAsync<TProjection>(
        Paging paging,
        Func<TEntity, TProjection> projection,
        string filter = null,
        string orderBy = null,
        CancellationToken cancellationToken = default)
        where TProjection : class;

    Task<TEntity> GetLastOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        CancellationToken cancellationToken = default);

    Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken);

    Task InsertAsync(TEntity entity, CancellationToken cancellationToken);

    Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);

    void Delete(TEntity entityToDelete);

    void Delete(IEnumerable<TEntity> entitiesToDelete);

    Task<BigInteger> GetSumAsync(
        Expression<Func<TEntity, long>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default);
}
namespace SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Application.DTOs.Pagination;

[ExcludeFromCodeCoverage]
public class GenericOperatorRepository<TEntity> : IGenericOperatorRepository<TEntity>
    where TEntity : class
{
    private readonly DbContext context;
    private readonly DbSet<TEntity> dbSet;

    public GenericOperatorRepository(
        DbContext context)
    {
        this.context = context;
        dbSet = context.Set<TEntity>();
    }

    public virtual async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        return filter != null
            ? await query.AnyAsync(filter, cancellationToken)
            : await query.AnyAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        return filter != null
            ? await query.SingleOrDefaultAsync(filter, cancellationToken)
            : await query.SingleOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return orderBy != null ?
            await orderBy(query).ToListAsync(cancellationToken) :
            await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<PaginatedResponseDto<TProjection>> GetProjectedPaginatedAsync<TProjection>(
        Paging paging,
        Func<TEntity, TProjection> projection,
        string filter = null,
        string orderBy = null,
        CancellationToken cancellationToken = default)
        where TProjection : class
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        var totalCount = query.Count();

        if (orderBy != null)
        {
            query = query.OrderBy(orderBy);
        }

        var entities = await query
            .Skip(paging.PageSize * (paging.PageNumber - 1))
            .Take(paging.PageSize)
            .ToDynamicListAsync<TEntity>(cancellationToken);

        return new PaginatedResponseDto<TProjection>(
            entities.Select(projection).ToList(),
            new MetadataDto(paging.PageNumber * paging.PageSize < totalCount, totalCount));
    }

    public virtual async Task<TEntity> GetLastOrDefaultAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return orderBy != null ?
            await orderBy(query).LastOrDefaultAsync(cancellationToken) :
            await query.LastOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TEntity> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        return await dbSet.FindAsync(new[] { id }, cancellationToken: cancellationToken);
    }

    public virtual async Task InsertAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await dbSet.AddAsync(entity, cancellationToken);
    }

    public virtual async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
    {
        await dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual void Delete(TEntity entityToDelete)
    {
        if (context.Entry(entityToDelete).State == EntityState.Detached)
        {
            dbSet.Attach(entityToDelete);
        }

        dbSet.Remove(entityToDelete);
    }

    public virtual void Delete(IEnumerable<TEntity> entitiesToDelete)
    {
        dbSet.RemoveRange(entitiesToDelete);
    }

    public virtual async Task<BigInteger> GetSumAsync(
        Expression<Func<TEntity, long>> selector,
        Expression<Func<TEntity, bool>> filter = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.SumAsync(selector, cancellationToken);
    }
}
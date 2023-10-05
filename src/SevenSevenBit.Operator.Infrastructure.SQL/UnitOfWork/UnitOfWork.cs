namespace SevenSevenBit.Operator.Infrastructure.SQL.UnitOfWork;

using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SevenSevenBit.Operator.Application.Common.Interfaces;
using SevenSevenBit.Operator.Infrastructure.SQL.Data.OperatorData;

[ExcludeFromCodeCoverage]
public class UnitOfWork : IUnitOfWork
{
    private readonly OperatorDbContext context;

    public UnitOfWork(OperatorDbContext context)
    {
        this.context = context;
    }

    public IGenericOperatorRepository<TEntity> Repository<TEntity>()
        where TEntity : class
        => new GenericOperatorRepository<TEntity>(context);

    public Task SaveAsync(CancellationToken cancellationToken)
        => context.SaveChangesAsync(cancellationToken);

    public Task RunSqlAsync(string sql, CancellationToken cancellationToken)
        => context.Database.ExecuteSqlRawAsync(sql, cancellationToken);
}
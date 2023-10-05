namespace SevenSevenBit.Operator.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IGenericOperatorRepository<TEntity> Repository<TEntity>()
        where TEntity : class;

    Task SaveAsync(CancellationToken cancellationToken);

    Task RunSqlAsync(string sql, CancellationToken cancellationToken);
}
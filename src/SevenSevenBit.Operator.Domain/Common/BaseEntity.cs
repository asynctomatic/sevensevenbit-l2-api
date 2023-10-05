namespace SevenSevenBit.Operator.Domain.Common;

using Microsoft.EntityFrameworkCore.Infrastructure;

public class BaseEntity
{
    public BaseEntity()
    {
    }

    protected BaseEntity(ILazyLoader lazyLoader)
    {
        LazyLoader = lazyLoader;
    }

    protected ILazyLoader LazyLoader { get; set; }
}
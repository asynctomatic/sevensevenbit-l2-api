namespace SevenSevenBit.Operator.Application.DTOs.Pagination;

public record Paging
{
    private const int UpperPageSize = 100;
    private const int DefaultPageSize = 10;
    private const int DefaultPageNumber = 1;

    public Paging()
    {
        PageNumber = DefaultPageNumber;
        PageSize = DefaultPageSize;
    }

    public Paging(int? pageNumber, int? pageSize)
    {
        PageNumber = pageNumber switch
        {
            null or < 1 => DefaultPageNumber,
            _ => pageNumber.Value,
        };

        PageSize = pageSize switch
        {
            null or < 1 => DefaultPageSize,
            > UpperPageSize => UpperPageSize,
            _ => pageSize.Value,
        };
    }

    public int PageNumber { get; }

    public int PageSize { get; }
}
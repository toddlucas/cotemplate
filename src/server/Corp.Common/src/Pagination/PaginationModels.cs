namespace Corp.Pagination;

public class PagedQuery
{
    public int Take { get; set; }
    public int Skip { get; set; }
    public string? Cursor { get; set; }
    public string? Search { get; set; }
    public string[]? Column { get; set; }
    public string[]? Direction { get; set; }
}

public class PagedResult<T>
{
    public T[] Items { get; set; } = [];
    public int Count { get; set; }
    public string? Next { get; set; }
}

public record PagedResult<T, C>(
    IReadOnlyCollection<T> Items,
    int Count,
    C Next)
{
    public static PagedResult<T, C> Create<T, C>(IReadOnlyCollection<T> items, int count, C next)
        => new(items, count, next);
}

public class PagedResult
{
    public static PagedResult<T, C> Create<T, C>(IReadOnlyCollection<T> items, int count, C next)
        => new(items, count, next);
}

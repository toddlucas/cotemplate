namespace Corp;

public class PagedResult<T>
{
    public T[] Items { get; set; } = [];
    public int Count { get; set; }
    public long? Next { get; set; }
}

public record PagedResult<T, C>(
    IReadOnlyCollection<T> Items,
    int Count,
    C Cursor)
{
}

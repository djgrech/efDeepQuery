namespace Common;

public record class PageResult<T>
{
    public int Total { get; set; }
    public List<T> Items { get; set; }

    public int Pages { get; set; }
    public int Page { get; set; }
}

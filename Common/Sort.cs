using System.Linq.Expressions;

namespace Common;

public class SortInput : List<KeyValuePair<string, SortDirection>>
{
    public SortInput Add(string key, SortDirection direction = SortDirection.Asc)
    {
        Add(new KeyValuePair<string, SortDirection>(key, direction));
        return this;
    }
}

public enum SortDirection
{
    Asc,
    Desc
}


public static class SortInputExtensions
{
    public static SortInput Add<T>(this SortInput sortInput, Expression<Func<T, object>> prop, SortDirection direction = SortDirection.Asc)
        => sortInput.Add(prop.GetPropertyName(), direction);
}
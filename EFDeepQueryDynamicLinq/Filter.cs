using EFDeepQueryDynamicLinq;
using System.Linq.Expressions;

namespace EFDeepQueryDynamicLinq;
public class FilterInput
{
    public List<FilterOperatorInput> Block { get; set; } = [];
}

public class FilterOperatorInput : List<KeyValuePair<string, FilterOperator>>
{
    public FilterOperatorInput Add(string key, FilterOperator item)
    {
        Add(new KeyValuePair<string, FilterOperator>(key, item));
        return this;
    }
}

public class FilterOperator
{
    public SearchOperator SearchOperator { get; set; } = SearchOperator.Equals;
    public List<object> Items { get; set; }
}

public enum SearchOperator
{
    Equals,
    Contains,
    GreaterThan,
    GreaterThanOrEquals,
    LessThan,
    LessThanOrEquals
}

public static class FilterOperatorExtensions
{

    public static FilterOperatorInput Add<T>(this FilterOperatorInput filterOperatorInput, Expression<Func<T, object>> key, FilterOperator item)
    {
        filterOperatorInput.Add(new KeyValuePair<string, FilterOperator>(key.GetPropertyName(), item));
        return filterOperatorInput;
    }

    public static FilterOperatorInput Add<T>(this FilterOperatorInput filterOperatorInput, Expression<Func<T, object>> key, Action<FilterOperator> filterOperatorAction, SearchOperator searchOperator = SearchOperator.Equals)
    {
        var filterOperator = new FilterOperator()
        {
            SearchOperator = searchOperator
        };
        filterOperatorAction(filterOperator);

        filterOperatorInput.Add(new KeyValuePair<string, FilterOperator>(key.GetPropertyName(), filterOperator));
        return filterOperatorInput;
    }

}
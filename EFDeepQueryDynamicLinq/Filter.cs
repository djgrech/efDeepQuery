using System.Linq.Expressions;

namespace EFDeepQueryDynamicLinq;

public enum LogicalOperator
{
    And,
    Or
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

public class FilterGroup
{
    public LogicalOperator Operator { get; set; } = LogicalOperator.And;
    public List<FilterGroup>? Groups { get; set; } = [];
    public List<FilterCondition>? Conditions { get; set; } = [];
}


public class FilterCondition
{
    public string Field { get; set; }
    public FilterOperator Operator { get; set; }

    public static FilterCondition Create<T>(Expression<Func<T, object>> prop, List<object> items, SearchOperator searchOperator = SearchOperator.Equals)
        => new()
        {
            Field = prop.GetPropertyName(),
            Operator = new FilterOperator()
            {
                Items = items,
                SearchOperator = searchOperator
            }
        };
}


public class FilterOperator
{
    public SearchOperator SearchOperator { get; set; } = SearchOperator.Equals;
    public List<object> Items { get; set; }
}

using System.Linq.Expressions;

namespace EFDeepQueryDynamicLinq;

public enum LogicalOperator
{
    And,
    Or
}

public interface IFilterComponent;

public class FilterGroup : IFilterComponent
{
    public LogicalOperator Operator { get; set; } = LogicalOperator.And;
    public List<IFilterComponent> Components { get; set; } = [];
}


public class FilterCondition : IFilterComponent
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

public enum SearchOperator
{
    Equals,
    Contains,
    GreaterThan,
    GreaterThanOrEquals,
    LessThan,
    LessThanOrEquals
}

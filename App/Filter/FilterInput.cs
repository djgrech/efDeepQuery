
namespace GraphQLSample.Filter;

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
    LessThan
}

public class FilterGroup
{
    public LogicalOperator Operator { get; set; }
    public List<FilterGroup>? Groups { get; set; }
    public List<FilterCondition>? Conditions { get; set; }
}

public class FilterCondition
{
    public string Field { get; set; }
    public FilterOperator Operator { get; set; }
}

public class FilterOperator
{
    public List<object> Items { get; set; }
    public SearchOperator SearchOperator { get; set; }
}

public class SortInput
{
    public string Key { get; set; }
    public SortDirection Value { get; set; }
}

public enum SortDirection
{
    Asc,
    Desc
}

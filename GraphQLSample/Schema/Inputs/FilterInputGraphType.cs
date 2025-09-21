using GraphQL;
using GraphQL.Types;
using GraphQLSample.Filter;

namespace GraphQLSample.Schema.Inputs;

public class SortInputType : InputObjectGraphType<SortInput>
{
    public SortInputType()
    {
        Field<StringGraphType, string>("key");
        Field<DirectionEnumerationEnumType, SortDirection>("value");
    }
}

public class FilterInputType : InputObjectGraphType<FilterInput>
{
    public FilterInputType()
    {
        Field<FilterGroupType, FilterGroup>("filter");
        Field<ListGraphType<SortInputType>, List<SortInput>>("sort");
    }
}

public class FilterInput
{
    public FilterGroup Filter { get; set; }
    public List<SortInput>? Sort { get; set; }
}

public class FilterGroupType : InputObjectGraphType<FilterGroup>
{
    public FilterGroupType()
    {
        Field<LogicalOperatorEnumType, LogicalOperator>("operator");
        Field<ListGraphType<FilterGroupType>, List<FilterGroup>>("groups");
        Field<ListGraphType<FilterConditionType>, List<FilterCondition>>("conditions");
    }
}

public class FilterConditionType : InputObjectGraphType<FilterCondition>
{
    public FilterConditionType()
    {
        Name = "FilterCondition";
        Field<StringGraphType, string>("field");
        Field<FilterOperatorType, FilterOperator>("operator");
    }
}

public class FilterOperatorType : InputObjectGraphType<FilterOperator>
{
    public FilterOperatorType()
    {
        Name = "FilterOperator";
        Field<ListGraphType<FilterValueType>, List<object>>("items");
        Field<SearchOperatorEnumType, SearchOperator>("searchOperator");
    }
}

public class FilterValueType : ScalarGraphType
{
    public override object? ParseValue(object? value)
        => value;
}

public class DirectionEnumerationEnumType : CamelCaseEnumGraphType<SortDirection>;

public class SearchOperatorEnumType : CamelCaseEnumGraphType<SearchOperator>;

public class LogicalOperatorEnumType : CamelCaseEnumGraphType<LogicalOperator>;


public abstract class CamelCaseEnumGraphType<T> : EnumerationGraphType<T>
    where T : Enum
{
    protected override string ChangeEnumCase(string val)
        => val.ToCamelCase();
}

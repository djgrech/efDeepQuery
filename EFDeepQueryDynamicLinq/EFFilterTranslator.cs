using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;

namespace EFDeepQueryDynamicLinq;

public interface IEFFilterTranslator
{
    IQueryable<TEntity> BuildQuery<TEntity>(IQueryable<TEntity> query, FilterGroup filterGroup, SortInput? sortInput = null)
            where TEntity : class;
}

public class EFFilterTranslator : IEFFilterTranslator
{
    private const string And = "AND";
    private const string Or = "OR";
    private const char OpenBrace = '(';
    private const char CloseBrace = ')';

    private readonly Dictionary<SortDirection, string> directionMap = new()
    {
        [SortDirection.Asc] = "ASC",
        [SortDirection.Desc] = "DESC"
    };

    private readonly Dictionary<SearchOperator, string> operatorMap = new()
    {
        [SearchOperator.Equals] = "=",
        [SearchOperator.LessThanOrEquals] = "<=",
        [SearchOperator.LessThan] = "<",
        [SearchOperator.GreaterThanOrEquals] = ">=",
        [SearchOperator.GreaterThan] = ">",
    };

    private readonly Dictionary<LogicalOperator, string> logicalOperatorMap = new()
    {
        [LogicalOperator.Or] = Or,
        [LogicalOperator.And] = And,
    };

    public IQueryable<TEntity> BuildQuery<TEntity>(IQueryable<TEntity> query, FilterGroup filterGroup, SortInput? sortInput = null)
            where TEntity : class
    {
        var f = new FilterMetaData();
        var queryStr = Build(filterGroup, f);

        foreach (var entity in f.ProcessedEntities)
            query = query.Include(entity);

        var parsedItems = f.Items.Select(item =>
        {
            if (item is JsonElement jsonElement)
            {
                if (jsonElement.ValueKind == JsonValueKind.String)
                {
                    var s = jsonElement.GetString(); ;
                    if (DateTime.TryParse(s, out var date))
                        return date.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");

                    return s;
                }


                if (jsonElement.ValueKind == JsonValueKind.Number && jsonElement.TryGetInt32(out var intVal))
                    return intVal;

                if (jsonElement.ValueKind == JsonValueKind.Number && jsonElement.TryGetDouble(out var doubleVal))
                    return doubleVal;

                if (jsonElement.ValueKind == JsonValueKind.True || jsonElement.ValueKind == JsonValueKind.False)
                    return jsonElement.GetBoolean();

                // fallback
                return jsonElement.ToString();
            }

            return item;
        }).ToList();

        query = query.Where(queryStr, [.. parsedItems]);

        if (!sortInput.IsNullOrEmpty())
            query = BuildOrderBy(query, sortInput!);

        return query;
    }


    private string Build(FilterGroup filterGroup, FilterMetaData filterMetaData, LogicalOperator? parentOperator = null)
    {
        var parts = new List<string>();

        if (filterGroup.Conditions?.Any() is true)
            foreach (var condition in filterGroup.Conditions)
                parts.Add(BuildCondition(condition, filterMetaData));

        if (filterGroup.Groups?.Any() is true)
            foreach (var group in filterGroup.Groups)
            {
                var subExpression = Build(group, filterMetaData, filterGroup.Operator);
                if (!string.IsNullOrEmpty(subExpression))
                    parts.Add(subExpression);
            }

        var expression = string.Join($" {filterGroup.Operator} ", parts);

        var isNeedParams = parentOperator != null && filterGroup.Operator != parentOperator || parts.Count > 1;

        return isNeedParams ? $"({expression})" : expression;
    }

    private string BuildCondition(FilterCondition condition, FilterMetaData metaData)
    {
        var field = condition.Field;
        var items = condition.Operator.Items;
        var op = condition.Operator.SearchOperator;

        if (items.IsNullOrEmpty())
            return string.Empty;

        var lastIndex = field.LastIndexOf('.');

        if (lastIndex != -1)
        {
            var entity = field[..lastIndex];
            metaData.ProcessedEntities.Add(entity);
        }

        var index = metaData.Index;

        var s = SubQuery(field, items, op, ref index, metaData.Items);
        metaData.Index = index;

        return s;
    }

    private string SubQuery(string key, List<object> items, SearchOperator searchOperator, ref int index, List<object> allItems)
    {
        var sb = new StringBuilder();
        if (items.Count > 0)
            sb.Append(OpenBrace);

        for (var i = 0; i < items.Count; i++)
        {
            if (i != 0)
                sb.Append($" {Or} ");

            if (searchOperator != SearchOperator.Contains)
                sb.Append($"{key} {operatorMap[searchOperator]} @{index++}");
            else
                sb.Append($"{key}.CONTAINS(@{index++})");
        }

        if (items.Count > 0)
            sb.Append(CloseBrace);

        allItems.AddRange(items);
        return sb.ToString();
    }

    private IQueryable<TEntity> BuildOrderBy<TEntity>(IQueryable<TEntity> query, SortInput sortInput)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < sortInput.Count; i++)
        {
            var sort = sortInput[i];
            if (i > 0)
                sb.Append(',');

            sb.Append($"{sort.Key} {directionMap[sort.Value]}");
        }

        return query.OrderBy(sb.ToString());
    }
}

internal record FilterMetaData
{
    public HashSet<string> ProcessedEntities { get; set; } = [];
    public List<object> Items { get; set; } = [];
    public int Index { get; set; }
}
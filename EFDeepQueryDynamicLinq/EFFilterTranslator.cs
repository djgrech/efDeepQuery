using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text;

namespace EFDeepQueryDynamicLinq;

public class EFFilterTranslator
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

    public IQueryable<TEntity> BuildQuery<TEntity>(IQueryable<TEntity> query, IFilterComponent component, SortInput? sortInput = null)
            where TEntity : class
    {
        var f = new FilterMetaData();
        var queryStr = BuildQuery(component, f);

        foreach (var entity in f.ProcessedEntities)
            query = query.Include(entity);

        query = query.Where(queryStr, [.. f.Items]);

        if (!sortInput.IsNullOrEmpty())
            query = BuildOrderBy(query, sortInput!);

        return query;
    }

    private string BuildQuery(IFilterComponent component, FilterMetaData filterMetaData)
    {
        if (component is FilterCondition condition)
            return BuildCondition(condition, filterMetaData);

        if (component is FilterGroup group)
        {
            var subQueries = group
                .Components
                .Select(x => BuildQuery(x, filterMetaData))
                .Where(q => !q.IsNullOrEmpty())
                .ToList();

            if (subQueries.IsNullOrEmpty())
                return string.Empty;

            var joined = string.Join($" {logicalOperatorMap[group.Operator]} ", subQueries);

            return $"({joined})";
        }

        return string.Empty;
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
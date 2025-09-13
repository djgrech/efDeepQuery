using System.Linq.Dynamic.Core;
using System.Text;

namespace EFDeepQueryDynamicLinq;

public class EFFilterTranslator
{
    private const string And = "AND";
    private const string Or = "Or";
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

    public IQueryable<TEntity> Build<TEntity>(IQueryable<TEntity> query, FilterInput filterInput, SortInput? sortInput = null)
                where TEntity : class
    {
        var f = new FilterMetaData();
        var init = true;

        var filterList = new List<string>();
        var sb = new StringBuilder();

        foreach (var block in filterInput.Block)
        {
            if (!init)
                sb.Append($" {Or} {OpenBrace}");
            else
            {
                init = false;
                sb.Append(OpenBrace);
            }

            sb.Append(BuildFilterOperatorString(f, block, And));
            sb.Append(CloseBrace);
        }

        query = query.Where(sb.ToString(), [.. f.Items]);
        if (!sortInput.IsNullOrEmpty())
            query = BuildOrderBy(query, sortInput!);

        return query;
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

    private string BuildFilterOperatorString(
        FilterMetaData metaData,
        FilterOperatorInput filterOperatorInput,
        string filterType
    )
    {
        var sb = new StringBuilder();
        var first = true;
        var index = metaData.Index;

        foreach (var filterOperator in filterOperatorInput)
        {
            var key = filterOperator.Key;
            var lastIndex = key.LastIndexOf('.');

            if (lastIndex != -1)
            {
                var entity = key[..lastIndex];
                metaData.ProcessedEntities.Add(entity);
            }

            if (!first)
                sb.Append($" {filterType} ");

            first = false;

            sb.Append(SubQuery(key, filterOperator.Value, ref index, metaData.Items));
        }

        metaData.Index = index;

        return sb.ToString();
    }

    private string SubQuery(string key, FilterOperator filterItem, ref int index, List<object> items)
    {
        var sb = new StringBuilder();
        if (filterItem.Items.Count > 0)
            sb.Append(OpenBrace);

        for (int i = 0; i < filterItem.Items.Count; i++)
        {
            if (i != 0)
                sb.Append(" OR ");

            if (filterItem.SearchOperator != SearchOperator.Contains)
                sb.Append($"{key} {operatorMap[filterItem.SearchOperator]} @{index++}");
            else
                sb.Append($"{key}.CONTAINS(@{index++})");
        }

        if (filterItem.Items.Count > 0)
            sb.Append(CloseBrace);

        items.AddRange(filterItem.Items);
        return sb.ToString();
    }
}

internal record FilterMetaData
{
    public HashSet<string> ProcessedEntities { get; set; } = [];
    public List<object> Items { get; set; } = [];
    public int Index { get; set; }
}
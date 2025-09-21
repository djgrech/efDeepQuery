using Common;

namespace DataDomain.Interfaces;

public interface IEFFilterTranslator
{
    IQueryable<TEntity> BuildQuery<TEntity>(IQueryable<TEntity> query, FilterGroup filterGroup, SortInput? sortInput = null)
            where TEntity : class;
}

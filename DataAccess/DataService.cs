using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public interface IDataService
{
    Task<List<T>> Query<T>(DbContext context, string include, string queryStr, List<string> values)
        where T : class;
}

public class DataService : IDataService
{
    public Task<List<T>> Query<T>(DbContext context, string include, string queryStr, List<string> values)
        where T : class
    {
        var dbSet = context.Set<T>();
        int lastIndex = include.LastIndexOf('.');
        var list = dbSet
            //.Include(include.Substring(0, lastIndex))
            .AsNoTracking() // not to use cached items - was returning inconsistent results
            .WhereNavigationPropertyInWithTrimAsync(queryStr, values);
     
        return list;
    }
}

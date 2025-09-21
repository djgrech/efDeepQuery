using Common;
using DataDomain.Interfaces.Domain;
using System.Linq.Expressions;

namespace DataDomain.Interfaces;

public interface IRepository<T> where T : class, IEntity
{
    Task<T?> GetByIdAsync(object id);
    Task<List<T>> Find(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task SaveAsync();
    Task<List<T>> GetFilteredData(FilterGroup filterGroup, SortInput? sortInput = null);
}

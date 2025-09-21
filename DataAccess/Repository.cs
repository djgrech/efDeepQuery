using Common;
using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess;

public abstract class Repository<T> : IRepository<T> where T : class, IEntity
{
    protected readonly DbContext _context;
    private readonly IEFFilterTranslator _filterTranslator;
    protected readonly DbSet<T> _dbSet;

    public Repository(DbContext context, IEFFilterTranslator filterTranslator)
    {
        _context = context;
        _filterTranslator = filterTranslator;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task<List<T>> GetFilteredData(FilterGroup filterGroup, SortInput? sortInput = null)
    {
        var query = _dbSet.AsQueryable<T>();
        return _filterTranslator.BuildQuery(query, filterGroup, sortInput).ToListAsync();
    }

    public Task<List<T>> Find(Expression<Func<T, bool>> predicate)
    {
        return _dbSet.Where(predicate).ToListAsync();
    }
}

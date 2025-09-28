using Common;
using DataDomain.Interfaces;
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
        => await _dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public async Task AddAsync(T entity)
        => await _dbSet.AddAsync(entity);

    public void Update(T entity)
        => _dbSet.Update(entity);

    public void Delete(T entity)
        => _dbSet.Remove(entity);

    public async Task SaveAsync()
        => await _context.SaveChangesAsync();

    public Task<List<T>> GetFilteredData(FilterGroup filterGroup, SortInput? sortInput = null)
    {
        var query = _dbSet.AsQueryable<T>();
        return _filterTranslator.BuildQuery(query, filterGroup, sortInput).ToListAsync();
    }

    public async Task<PageResult<T>> GetFilteredPagedData(FilterGroup filterGroup, PageInput pageInput, SortInput? sortInput = null)
    {
        var query = _dbSet.AsQueryable<T>();

        query = _filterTranslator.BuildQuery(query, filterGroup, sortInput);
        var totalItems = query.Count();

        if (pageInput != null)
            query = query
                .Skip(pageInput.PageNo * pageInput.Page)
                .Take(pageInput.Page);

        var items = await query.ToListAsync();

        return new PageResult<T>()
        {
            Items = items,
            Page = pageInput!.PageNo,
            Pages = GetNumberOfPages(totalItems, pageInput.PageNo),
            Total = totalItems
        };
    }

    private static int GetNumberOfPages(int totalItems, int pageNo)
    {
        int pages = totalItems / pageNo;
        if ((totalItems % pageNo) != 0)
            pages++;

        return pages;
    }

    public Task<List<T>> Find(Expression<Func<T, bool>> predicate)
        => _dbSet.Where(predicate).ToListAsync();
}

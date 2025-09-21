using AutoMapper;
using Common;
using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;

namespace DataService.Services;

public interface IBaseService<T, TEntity>
{
    Task<T?> GetById(int id);
    Task<List<T>> GetByIds(HashSet<int> ids);
    Task<List<T>> GetFilteredData(FilterGroup filterGroup, SortInput? sortInput = null);
}

public abstract class BaseService<T, TEntity> : IBaseService<T, TEntity>
    where TEntity : class, IEntity
{
    protected readonly IRepository<TEntity> Repository;
    protected readonly IMapper Mapper;

    public BaseService(IRepository<TEntity> repository, IMapper mapper)
    {
        this.Repository = repository;
        this.Mapper = mapper;
    }

    public async Task<T?> GetById(int id)
    {
        var entity = await Repository.GetByIdAsync(id);

        return entity == null
            ? default
            : Mapper.Map<T>(entity);
    }

    public async Task<List<T>> GetByIds(HashSet<int> ids)
        => Mapper.Map<List<T>>(await Repository.Find(x => ids.Contains(x.Id)));

    public async Task<List<T>> GetFilteredData(FilterGroup filterGroup, SortInput? sortInput = null)
    {
        var list = await Repository.GetFilteredData(filterGroup, sortInput);
        return Mapper.Map<List<T>>(list);
    }
}

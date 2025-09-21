using AutoMapper;
using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using DataService.DTOs;
using EFDeepQueryDynamicLinq;

namespace DataService.Services;

public interface IOrderService : IBaseService<OrderDTO, Order>
{
    Task<List<OrderDTO>> GetByCustomerIds(HashSet<int> ids);
}

public class OrderService(IOrderRepository repository, IMapper mapper)
    : BaseService<OrderDTO, Order>(repository, mapper), IOrderService
{
    public async Task<List<OrderDTO>> GetByCustomerIds(HashSet<int> ids)
    {
        return Mapper.Map<List<OrderDTO>>(await Repository.Find(x => ids.Contains(x.CustomerId)));
    }
}

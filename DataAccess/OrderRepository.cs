using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using EFDeepQueryDynamicLinq;

namespace DataAccess;

public class OrderRepository : Repository<OrderEntity>, IOrderRepository
{
    public OrderRepository(ApplicationContext context, IEFFilterTranslator eFFilterTranslator) : base(context, eFFilterTranslator)
    {
    }
}

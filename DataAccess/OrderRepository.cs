using DataDomain.Interfaces;
using DataDomain.OrderDomain;

namespace DataAccess;

public class OrderRepository(ApplicationContext context, IEFFilterTranslator eFFilterTranslator) 
    : Repository<OrderEntity>(context, eFFilterTranslator), IOrderRepository
{
}

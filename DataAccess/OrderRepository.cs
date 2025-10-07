using DataDomain;
using DataDomain.Interfaces;

namespace DataAccess;

public class OrderRepository(ApplicationContext context, IEFFilterTranslator eFFilterTranslator) 
    : Repository<OrderEntity>(context, eFFilterTranslator), IOrderRepository
{
}

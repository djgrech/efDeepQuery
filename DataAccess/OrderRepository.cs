using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;

namespace DataAccess;

public class OrderRepository(ApplicationContext context, IEFFilterTranslator eFFilterTranslator) 
    : Repository<OrderEntity>(context, eFFilterTranslator), IOrderRepository
{
}

using DataDomain.Interfaces;
using DataDomain.OrderDomain;

namespace DataAccess;

public class CustomerRepository(ApplicationContext context, IEFFilterTranslator filterTranslator)
    : Repository<CustomerEntity>(context, filterTranslator), ICustomerRepository
{
}

using DataDomain;
using DataDomain.Interfaces;

namespace DataAccess;

public class CustomerRepository(ApplicationContext context, IEFFilterTranslator filterTranslator)
    : Repository<CustomerEntity>(context, filterTranslator), ICustomerRepository
{
}

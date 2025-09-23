using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;

namespace DataAccess;

public class CustomerRepository(ApplicationContext context, IEFFilterTranslator filterTranslator)
    : Repository<CustomerEntity>(context, filterTranslator), ICustomerRepository
{
}

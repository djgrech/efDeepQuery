using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;

namespace DataAccess;

public class CustomerRepository : Repository<CustomerEntity>, ICustomerRepository
{
    public CustomerRepository(ApplicationContext context, IEFFilterTranslator filterTranslator) : base(context, filterTranslator)
    {
    }
}

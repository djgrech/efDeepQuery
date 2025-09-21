using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;

namespace DataAccess;

public class CustomerRepository : Repository<Customer>, ICustomerRepository
{
    public CustomerRepository(ApplicationContext context, IEFFilterTranslator filterTranslator) : base(context, filterTranslator)
    {
    }
}

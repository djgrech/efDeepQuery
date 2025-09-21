using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using EFDeepQueryDynamicLinq;

namespace DataAccess;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationContext context, IEFFilterTranslator filterTranslator) : base(context, filterTranslator)
    {
    }
}

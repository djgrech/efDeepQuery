using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;
using EFDeepQueryDynamicLinq;

namespace DataAccess;

public class ProductRepository : Repository<ProductEntity>, IProductRepository
{
    public ProductRepository(ApplicationContext context, IEFFilterTranslator filterTranslator) : base(context, filterTranslator)
    {
    }
}

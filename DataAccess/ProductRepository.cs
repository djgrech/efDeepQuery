using DataDomain;
using DataDomain.Interfaces;

namespace DataAccess;

public class ProductRepository(ApplicationContext context, IEFFilterTranslator filterTranslator)
    : Repository<ProductEntity>(context, filterTranslator), IProductRepository
{
}

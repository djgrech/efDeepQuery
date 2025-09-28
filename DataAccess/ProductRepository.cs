using DataDomain.Interfaces;
using DataDomain.OrderDomain;

namespace DataAccess;

public class ProductRepository(ApplicationContext context, IEFFilterTranslator filterTranslator)
    : Repository<ProductEntity>(context, filterTranslator), IProductRepository
{
}

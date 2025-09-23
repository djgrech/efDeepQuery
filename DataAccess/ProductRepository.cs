using DataDomain.Interfaces;
using DataDomain.Interfaces.Domain;

namespace DataAccess;

public class ProductRepository(ApplicationContext context, IEFFilterTranslator filterTranslator)
    : Repository<ProductEntity>(context, filterTranslator), IProductRepository
{
}

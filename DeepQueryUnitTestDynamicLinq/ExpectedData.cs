
using DataDomain.Interfaces.Domain;

namespace DeepQueryUnitTestDynamicLinq;

public class ExpectedData
{
    public OrderEntity Order { get; set; }
    public CustomerEntity Customer { get; set; }
    public ProductEntity Product { get; set; }
    public List<OrderEntity> Orders { get; set; }
}


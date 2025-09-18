using GraphQL.Types;
using GraphQLSample.Clients;

namespace GraphQLSample.Schema;

public abstract class BaseGraphType<T> : ObjectGraphType<T>
    where T : ResponseBase
{
    protected BaseGraphType()
    {
        Field(x => x.Id);
    }
}

public class OrderGraphType : BaseGraphType<OrderResponse>
{
    public OrderGraphType()
    {
        Name = "order";
        Field(x => x.OrderDate);

        Field<ProductGraphType, Product>("product").Resolve(x => x.Source.Product);
        Field<Customer1GraphType, Customer>("customer").Resolve(x => x.Source.Customer);
    }
}

public class Customer1GraphType : BaseGraphType<Customer>
{
    public Customer1GraphType()
    {
        Name = "customer";
        Field(x => x.FirstName);
        Field(x => x.LastName);
    }
}

public class ProductGraphType : BaseGraphType<Product>
{
    public ProductGraphType()
    {
        Name = "product";
        Field(x => x.Name);
    }
}

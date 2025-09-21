using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using GraphQLSample.Clients;
using GraphQLSample.Services;

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
    public OrderGraphType(IServiceProvider serviceProvider)
    {
        Name = "order";
        Field(x => x.OrderDate);

        Field<ProductGraphType, ProductResponse>("product").ResolveAsync(ctx => ctx.QueryOneToOne<ProductResponse>(serviceProvider, "productById", ctx.Source.ProductId));
        Field<CustomerGraphType, CustomerResponse>("customer").ResolveAsync(ctx => ctx.QueryOneToOne<CustomerResponse>(serviceProvider, "customertById", ctx.Source.CustomerId));
    }
}


public class CustomerGraphType : BaseGraphType<CustomerResponse>
{
    public CustomerGraphType(IServiceProvider serviceProvider)
    {
        Name = "customer";
        Field(x => x.FirstName);
        Field(x => x.LastName);
        Field<ListGraphType<OrderGraphType>, IEnumerable<OrderResponse>>("orders").ResolveAsync(ctx =>
        ctx.QueryManyToOne<OrderResponse, IOrdersDataService>(
            serviceProvider,
            "orderByCustomerId",
            ctx.Source.Id,
            x => x.CustomerId,
            (service, ids) => service.GetByCustomerIds([.. ids])
        ));
    }
}

public class ProductGraphType : BaseGraphType<ProductResponse>
{
    public ProductGraphType()
    {
        Name = "product";
        Field(x => x.Name);
    }
}

using GraphQLSample.Filter;

namespace GraphQLSample.Clients;

public class Request
{
    public FilterGroup Filter { get; set; }
    public List<SortInput>? Sort { get; set; }
}

public abstract class ResponseBase
{
    public int Id { get; set; }
}

public class OrderResponse : ResponseBase
{
    public DateTime OrderDate { get; set; }

    public int ProductId { get; set; }

    public int CustomerId { get; set; }
}

public class ProductResponse : ResponseBase
{
    public string Name { get; set; }
}

public class CustomerResponse : ResponseBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

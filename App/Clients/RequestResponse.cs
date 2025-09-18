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
    public Product Product { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
}

public class Product : ResponseBase
{
    public string Name { get; set; }
}

public class Customer : ResponseBase
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

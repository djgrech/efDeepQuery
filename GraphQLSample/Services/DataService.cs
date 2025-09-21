using GraphQLSample.Clients;

namespace GraphQLSample.Services;

public interface IDataService<T>
{
    Task<List<T>> Get(Request request);
    Task<List<T>> GetByIds(List<int> ids);
}

public class DataService<T> : IDataService<T>
{
    protected readonly IDataHttpClient<T> client;

    public DataService(IDataHttpClient<T> client)
    {
        this.client = client;
    }

    public Task<List<T>> Get(Request request)
        => client.Get(request);

    public Task<List<T>> GetByIds(List<int> ids)
        => client.GetByIds(ids);
}

public interface IOrdersDataService : IDataService<OrderResponse>
{
    Task<List<OrderResponse>> GetByCustomerIds(List<int> customerIds);
}


public class OrdersDataService(IOrderDataHttpClient client) : DataService<OrderResponse>(client), IOrdersDataService
{
    public Task<List<OrderResponse>> GetByCustomerIds(List<int> customerIds)
    {
        var client = (IOrderDataHttpClient)(base.client);
        return client.GetByCustomerIds(customerIds);
    }
}
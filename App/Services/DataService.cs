using GraphQLSample.Clients;

namespace GraphQLSample.Services;

public interface IDataService
{
    Task<List<OrderResponse>> Get(Request request);
}

public class DataService : IDataService
{
    private readonly IDataHttpClient client;

    public DataService(IDataHttpClient client)
    {
        this.client = client;
    }

    public Task<List<OrderResponse>> Get(Request request)
    {
        return client.Get(request);
    }
}

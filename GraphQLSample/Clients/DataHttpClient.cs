using System.Text.Json;
using System.Text.Json.Serialization;

namespace GraphQLSample.Clients;

public interface IDataHttpClient<T>
{
    Task<List<T>> Get(Request request);
    Task<List<T>> GetByIds(List<int> ids);
}

public interface IOrderDataHttpClient : IDataHttpClient<OrderResponse>
{
    Task<List<OrderResponse>> GetByCustomerIds(List<int> ids);
}

public abstract class DataHttpClient<T>(IHttpClientFactory httpFactory) : IDataHttpClient<T>
{
    protected readonly HttpClient client = httpFactory.CreateClient("efDeepQuery");

    protected virtual string Resource { get; set; }

    protected readonly JsonSerializerOptions options = new()
    {
        Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
        PropertyNameCaseInsensitive = true
    };

    public async Task<List<T>> Get(Request request)
    {
        var content = JsonSerializer.Serialize(request, options);
        var result = await client.PostAsync(Resource, GetContent(content));

        var response = await result.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<T>>(response, options) ?? [];
    }

    public async Task<List<T>> GetByIds(List<int> ids)
    {
        var content = JsonSerializer.Serialize(ids, options);

        var result = await client.PostAsync($"{Resource}/ids", GetContent(content));

        var response = await result.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<T>>(response, options) ?? [];
    }

    protected StringContent GetContent(string content)
        => new StringContent(content, System.Text.Encoding.UTF8, "application/json");
}

public class CustomerHttpClient : DataHttpClient<CustomerResponse>, IDataHttpClient<CustomerResponse>
{
    public CustomerHttpClient(IHttpClientFactory httpFactory) : base(httpFactory)
    {
        Resource = "customer";
    }
}

public class ProductHttpClient : DataHttpClient<ProductResponse>, IDataHttpClient<ProductResponse>
{
    public ProductHttpClient(IHttpClientFactory httpFactory) : base(httpFactory)
    {
        Resource = "product";
    }
}

public class OrderHttpClient : DataHttpClient<OrderResponse>, IOrderDataHttpClient
{
    public OrderHttpClient(IHttpClientFactory httpFactory) : base(httpFactory)
    {
        Resource = "order";
    }

    public async Task<List<OrderResponse>> GetByCustomerIds(List<int> ids)
    {
        var content = JsonSerializer.Serialize(ids, options);

        var result = await client.PostAsync($"{Resource}/byCustomerIds", GetContent(content));

        var response = await result.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<OrderResponse>>(response, options) ?? [];
    }
}
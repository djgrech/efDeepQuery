using System.Text.Json;
using System.Text.Json.Serialization;

namespace GraphQLSample.Clients;

public interface IDataHttpClient
{
    Task<List<OrderResponse>> Get(Request request);
}

public class DataHttpClient : IDataHttpClient
{
    private readonly IHttpClientFactory httpFactory;

    public DataHttpClient(IHttpClientFactory httpFactory)
    {
        this.httpFactory = httpFactory;
    }

    public async Task<List<OrderResponse>> Get(Request request)
    {
        var client = httpFactory.CreateClient("efDeepQuery");
        var options = new JsonSerializerOptions()
        {
            Converters =
                {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
            PropertyNameCaseInsensitive = true
        };

        var content = JsonSerializer.Serialize(request, options);

        var result = await client.PostAsync("data", new StringContent(content, System.Text.Encoding.UTF8, "application/json"));

        var response = await result.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<List<OrderResponse>>(response, options) ?? [];
    }
}

using GraphQL.Types;
using GraphQLSample.Clients;
using GraphQLSample.Extensions;
using GraphQLSample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAppGraphQL();
builder.Services.AddSingleton(typeof(IDataService<>), typeof(DataService<>));
//builder.Services.AddSingleton(typeof(IDataHttpClient<>), typeof(DataHttpClient<>));
builder.Services.AddSingleton<IDataHttpClient<OrderResponse>, OrderHttpClient>();
builder.Services.AddSingleton<IDataHttpClient<CustomerResponse>, CustomerHttpClient>();
builder.Services.AddSingleton<IDataHttpClient<ProductResponse>, ProductHttpClient>();


builder.Services.AddSingleton<IOrderDataHttpClient, OrderHttpClient>();
builder.Services.AddSingleton<IOrdersDataService, OrdersDataService>();
var clientUrl = builder.Configuration.GetValue<string>("application:url");

builder.Services.AddHttpClient("efDeepQuery", client =>
{
    client.BaseAddress = new Uri(clientUrl);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseRouting();
app.UseGraphQL<ISchema>();
app.UseGraphQLGraphiQL(); // https://localhost:7186/ui/graphiql
app.Run();

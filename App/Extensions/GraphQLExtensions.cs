using GraphQL;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using GraphQLSample.Schema;

namespace GraphQLSample.Extensions;

public static class GraphQLExtensions
{
    public static IServiceCollection AddAppGraphQL(this IServiceCollection services)
    {
        services.AddGraphQL(options =>
        {
            options.AddSchema<AppSchema>();
            options.AddGraphTypes();
            options.AddSystemTextJson();
            options.AddDataLoader();
            //options.Tool.Enable = true;
        });


        services.AddSingleton<AppGraphQuery>();
        services.AddSingleton<ISchema, AppSchema>();
        services.AddSingleton<IGraphQLTextSerializer, GraphQLSerializer>();

        return services;
    }
}


namespace GraphQLSample.Schema;

public class AppSchema : GraphQL.Types.Schema
{
    public AppSchema(IServiceProvider provider)
        : base(provider)
    {
        Query = provider.GetRequiredService<AppGraphQuery>();
    }
}

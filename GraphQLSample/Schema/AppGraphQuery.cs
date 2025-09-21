using GraphQL;
using GraphQL.Types;
using GraphQLSample.Clients;
using GraphQLSample.Models;
using GraphQLSample.Schema.Inputs;
using GraphQLSample.Services;

namespace GraphQLSample.Schema;

public class AppGraphQuery : ObjectGraphType
{
    public AppGraphQuery(IServiceProvider serviceProvider)
    {
        Field<HelloWorldType, HelloWorld>("hello")
            .Argument<HelloInputType>("input")
            .Resolve(ctx =>
            {
                var input = ctx.GetArgument<HelloParam>("input");
                return new HelloWorld();
            });

        Field<ListGraphType<OrderGraphType>, List<OrderResponse>>("orders")
            .Argument<FilterInputType>("input")
            .ResolveAsync(async ctx =>
            {
                var input = ctx.GetArgument<FilterInput>("input");

                var dataService = serviceProvider.GetRequiredService<IDataService<OrderResponse>>();

                var result = await dataService.Get(new Request()
                {
                    Filter = input.Filter,
                    Sort = input.Sort
                });

                return result;
            });

        Field<ListGraphType<CustomerGraphType>, List<CustomerResponse>>("customers")
            .Argument<FilterInputType>("input")
            .ResolveAsync(async ctx =>
            {
                var input = ctx.GetArgument<FilterInput>("input");

                var dataService = serviceProvider.GetRequiredService<IDataService<CustomerResponse>>();
                var result = await dataService.Get(new Request()
                {
                    Filter = input.Filter,
                    Sort = input.Sort
                });

                return result;
            });

    }
}

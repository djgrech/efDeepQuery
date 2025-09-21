using GraphQL;
using GraphQL.DataLoader;
using GraphQLSample.Clients;
using GraphQLSample.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace GraphQLSample.Schema;

public static class DataLoaderExtensions
{
    public static IDataLoaderResult<T> QueryOneToOne<T>(this IResolveFieldContext context, IServiceProvider serviceProvider, string key, int id)
        where T : ResponseBase
    {
        var service = serviceProvider.GetRequiredService<IDataService<T>>();
        var dataLoaderContext = serviceProvider.GetRequiredService<IDataLoaderContextAccessor>();

        var loader = dataLoaderContext.Context.GetOrAddBatchLoader<int, T>(key, async ids =>
        {
            var list = await service.GetByIds([.. ids]);

            return list.ToDictionary(x => x.Id);

        });

        return loader.LoadAsync(id);
    }

    public static IDataLoaderResult<IEnumerable<T>> QueryManyToOne<T, TService>(
        this IResolveFieldContext context,
        IServiceProvider serviceProvider,
        string key,
        int id,
        Func<T, int> keyProp,
        Func<TService, IEnumerable<int>, Task<List<T>>> queryFunc
    ) where T : ResponseBase
    {
        var service = serviceProvider.GetService<TService>();
        if (service == null)
            throw new ArgumentNullException(nameof(service));

        var dataLoaderContext = serviceProvider.GetRequiredService<IDataLoaderContextAccessor>();

        var loader = dataLoaderContext.Context.GetOrAddCollectionBatchLoader<int, T>(key, async ids =>
        {
            var list = await queryFunc(service, ids);

            return list.ToLookup(keyProp);

        });

        return loader.LoadAsync(id);
    }
    /*
    
        {
            var orderDataService = serviceProvider.GetRequiredService<IOrdersDataService>();

    var loader = dataLoaderContext.Context.GetOrAddCollectionBatchLoader<int, OrderResponse>("orderByCustomerId", async ids =>
    {
        var list = await orderDataService.GetByCustomerIds([.. ids]);

        return list.ToLookup(x => x.CustomerId);

    });

            return loader.LoadAsync(ctx.Source.Id);
        });*/

}

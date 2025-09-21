using DataAccess;
using DataDomain.Interfaces;
using DataService.DTOs;
using DataService.Services;
using EFDeepQueryDynamicLinq;
using Microsoft.Extensions.DependencyInjection;

namespace DataService;

public static class AutoMapperExtensions
{
    public static IServiceCollection AddAutoMapping(this IServiceCollection services)
    {
        services.AddAutoMapper(c =>
        {
            c.AddProfile<OrderToOrderDTOMappingProfile>();
            c.AddProfile<CustomerToCustomerDTOMappingProfile>();
            c.AddProfile<ProductToProductDTOMappingProfile>();
        }
        );
        return services;
    }
}

public static class AddDataServicesExtensions
{
    public static IServiceCollection AddDataServices(this IServiceCollection services)
    {
        return services
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IProductService, ProductService>()
            .AddSingleton<IEFFilterTranslator, EFFilterTranslator>();
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IOrderRepository, OrderRepository>()
            .AddScoped<IProductRepository, ProductRepository>();
    }
}

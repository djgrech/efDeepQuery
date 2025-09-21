using Common;
using DataAccess;
using DataDomain.Interfaces.Domain;
using DeepQueryUnitTestDynamicLinq.TestData;
using EFDeepQueryDynamicLinq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace DeepQueryUnitTestDynamicLinq;

public class DeepQueryTest
{
    [Theory, ClassData(typeof(OrderTestData))]
    public void Test_Query_By_Order(FilterGroup filterInput, SortInput? sortInput, List<ExpectedData> expectedData)
    {
        var filterTranslator = new EFFilterTranslator();

        var context = GetContext();
        var query = context.Set<OrderEntity>().AsQueryable();
        query = filterTranslator.BuildQuery(query, filterInput, sortInput);

        var result = query.ToList();

        result.Count.Should().Be(expectedData.Count);
        if (expectedData.Count != 0)
            result.Should().SatisfyRespectively(GetOrderAsseration(expectedData));
    }

    [Theory, ClassData(typeof(CustomerTestData))]
    public void Test_Query_By_Customer(FilterGroup filterInput, List<ExpectedData> expectedData)
    {
        var filterTranslator = new EFFilterTranslator();

        var context = GetContext();
        var query = context.Set<CustomerEntity>().AsQueryable();
        query = filterTranslator.BuildQuery(query, filterInput);

        var result = query.ToList();

        result.Count.Should().Be(expectedData.Count);
        if (expectedData.Count != 0)
            result.Should().SatisfyRespectively(GetCustomerAsseration(expectedData));
    }

    [Theory, ClassData(typeof(ProductTestData1))]
    public void Test_Query_By_Product(FilterGroup filterInput, SortInput? sortInput, List<ExpectedData> expectedData)
    {
        var filterTranslator = new EFFilterTranslator();

        var context = GetContext();
        var query = context.Set<ProductEntity>().AsQueryable();
        query = filterTranslator.BuildQuery(query, filterInput, sortInput);

        var result = query.ToList();

        result.Count.Should().Be(expectedData.Count);
        if (expectedData.Count != 0)
            result.Should().SatisfyRespectively(GetProductAsseration(expectedData));
    }

    private static Action<OrderEntity>[] GetOrderAsseration(List<ExpectedData> expectedData)
        => [.. expectedData.Select(item => new Action<OrderEntity>(
                x =>
                {
                    x.OrderDate.Should().Be(item.Order.OrderDate);
                    x.Id.Should().Be(item.Order.Id);
                    x.Product.Name.Should().Be(item.Product.Name);
                    x.Product.Id.Should().Be(item.Product.Id);
                    x.CustomerId.Should().Be(item.Customer.Id);
                    x.Customer.FirstName.Should().Be(item.Customer.FirstName);
                    x.Customer.LastName.Should().Be(item.Customer.LastName);
                    x.Customer.Id.Should().Be(item.Customer.Id);
                }))];

    private static Action<CustomerEntity>[] GetCustomerAsseration(List<ExpectedData> expectedData)
        => [.. expectedData.Select(item => new Action<CustomerEntity>(
                x =>
                {
                    x.Id.Should().Be(item.Customer.Id);
                    x.FirstName.Should().Be(item.Customer.FirstName);
                    x.LastName.Should().Be(item.Customer.LastName);
                    x.Orders.Count.Should().Be(item.Orders.Count);

                    x.Orders.Should().SatisfyRespectively(GetOrderAsseration([.. item.Orders.Select(order => new ExpectedData()
                    {
                        Order = order,
                        Customer = new CustomerEntity()
                        {
                            Id = item.Customer.Id,
                            FirstName = item.Customer.FirstName,
                            LastName = item.Customer.LastName,
                        },
                        Product = new ProductEntity()
                        {
                            Id = order.Product.Id,
                            Name = order.Product.Name,
                        }
                    })]));
                }))];

    private static Action<ProductEntity>[] GetProductAsseration(List<ExpectedData> expectedData)
        => [.. expectedData.Select(item => new Action<ProductEntity>(
            x =>
            {
                x.Id.Should().Be(item.Product.Id);
                x.Name.Should().Be(item.Product.Name);

                x.Orders.Should().SatisfyRespectively(GetOrderAsseration([.. item.Product.Orders.Select(order => new ExpectedData()
                    {
                        Order = order,
                        Customer = new CustomerEntity()
                        {
                            Id = order.Customer.Id,
                            FirstName = order.Customer.FirstName,
                            LastName = order.Customer.LastName,
                        },
                        Product = new ProductEntity()
                        {
                            Id = item.Product.Id,
                            Name = item.Product.Name,
                        }
                })]));
            }))];

    private static ApplicationContext GetContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationContext(options);

        context.Customers.AddRange(MockData.Customers);
        context.Products.AddRange(MockData.Products);
        context.Orders.AddRange(MockData.Orders);

        context.SaveChanges();

        return context;
    }
}

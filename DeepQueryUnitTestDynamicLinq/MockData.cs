using DataDomain.Order;

namespace DeepQueryUnitTestDynamicLinq;

public static class MockData
{
    public static List<Product> Products =
    [
        new Product()
        {
            Id = 1,
            Name = "keyboard",
        },
        new Product()
        {
            Id = 2,
            Name = "mouse"
        },
        new Product()
        {
            Id = 3,
            Name = "monitor"
        }
    ];

    public static List<Order> Orders =
    [
        new Order()
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = new DateTime(2025,1,1),
            ProductId = 1
        },
        new Order()
        {
            Id = 2,
            CustomerId = 1,
            OrderDate = new DateTime(2025,2,1),
            ProductId = 2
        },
        new Order()
        {
            Id = 3,
            CustomerId = 2,
            OrderDate = new DateTime(2025,3,1),
            ProductId = 2
        },
        new Order()
        {
            Id = 4,
            CustomerId = 3,
            OrderDate = new DateTime(2025,3,1),
            ProductId = 3
        }
    ];

    public static List<Customer> Customers =
    [
        new Customer()
        {
            Id = 1,
            FirstName = "Joe",
            LastName = "Borg"
        },
        new Customer()
        {
            Id = 2,
            FirstName = "Mary",
            LastName = "Smith"
        },
        new Customer()
        {
            Id = 3,
            FirstName = "Mary",
            LastName = "Black"
        }
    ];
}

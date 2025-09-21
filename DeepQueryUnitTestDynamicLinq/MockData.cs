using DataDomain.Interfaces.Domain;

namespace DeepQueryUnitTestDynamicLinq;

public static class MockData
{
    public static List<ProductEntity> Products =
    [
        new ProductEntity()
        {
            Id = 1,
            Name = "keyboard",
        },
        new ProductEntity()
        {
            Id = 2,
            Name = "mouse"
        },
        new ProductEntity()
        {
            Id = 3,
            Name = "monitor"
        }
    ];

    public static List<OrderEntity> Orders =
    [
        new OrderEntity()
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = new DateTime(2025,1,1),
            ProductId = 1
        },
        new OrderEntity()
        {
            Id = 2,
            CustomerId = 1,
            OrderDate = new DateTime(2025,2,1),
            ProductId = 2
        },
        new OrderEntity()
        {
            Id = 3,
            CustomerId = 2,
            OrderDate = new DateTime(2025,3,1),
            ProductId = 2
        },
        new OrderEntity()
        {
            Id = 4,
            CustomerId = 3,
            OrderDate = new DateTime(2025,3,1),
            ProductId = 3
        }
    ];

    public static List<CustomerEntity> Customers =
    [
        new CustomerEntity()
        {
            Id = 1,
            FirstName = "Joe",
            LastName = "Borg"
        },
        new CustomerEntity()
        {
            Id = 2,
            FirstName = "Mary",
            LastName = "Smith"
        },
        new CustomerEntity()
        {
            Id = 3,
            FirstName = "Mary",
            LastName = "Black"
        }
    ];
}

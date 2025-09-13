using DataDomain.Order;
using EFDeepQueryDynamicLinq;

namespace DeepQueryUnitTestDynamicLinq.TestData;

public class OrderTestData : TheoryData<FilterInput, List<ExpectedData>>
{
    public OrderTestData()
    {
        Add(new FilterInput
        {
            Block =
            [
                new FilterOperatorInput()
                .Add<Order>(x => x.Product.Name, x => x.Items = ["mou", "key"], SearchOperator.Contains)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                .Add<Order>(x => x.Customer.FirstName, x => x.Items = ["Mary"])
                .Add<Order>(x => x.Customer.LastName, x => x.Items = ["Smith"]),
            ]
        },
        [
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 3,
                    CustomerId= 2,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                Product = new Product()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new Customer()
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Smith"
                }
            }
         ]);

        Add(new FilterInput
        {
            Block =
            [
               new FilterOperatorInput()
                .Add<Order>(x => x.Product.Name, x => x.Items = ["mou", "key"], SearchOperator.Contains)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                .Add<Order>(x => x.Customer.FirstName, x => x.Items = ["Mary"])
                .Add<Order>(x => x.Customer.LastName, x => x.Items = ["Smith"]),

                new FilterOperatorInput()
                .Add<Order>(x => x.Id, x => x.Items = [1,3])
                .Add<Order>(x=>x.Customer.FirstName, x=>x.Items = ["Jo"], SearchOperator.Contains)
            ]
        },
        [
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate =new DateTime(2025, 1, 1)
                },
                Product = new Product()
                {
                    Id = 1,
                    Name = "keyboard"
                },
                Customer = new Customer()
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Borg"
                }
            },
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate =new DateTime(2025, 3, 1)
                },
                Product = new Product()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new Customer()
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Smith"
                }
            }
        ]);

        Add(new FilterInput
        {
            Block =
            [
               new FilterOperatorInput()
                .Add<Order>(x => x.Product.Name, x => x.Items = ["mou", "key"], SearchOperator.Contains)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                .Add<Order>(x => x.Customer.FirstName, x => x.Items = ["Mary"])
                .Add<Order>(x => x.Customer.LastName, x => x.Items = ["Smith"]),

            new FilterOperatorInput()
                .Add<Order>(x => x.Id, x => x.Items = [1,2])
                .Add<Order>(x=>x.Customer.FirstName, x=>x.Items = ["Jo"], SearchOperator.Contains)
            ]
        },
        [
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = new DateTime(2025, 1, 1)
                },
                Product = new Product()
                {
                    Id = 1,
                    Name = "keyboard"
                },
                Customer = new Customer()
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Borg"
                }
            },
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate =new DateTime(2025, 2, 1)
                },
                Product = new Product()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new Customer()
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Borg"
                }
            },
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate =new DateTime(2025, 3, 1)
                },
                Product = new Product()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new Customer()
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Smith"
                }
            }
        ]);

        Add(new FilterInput
        {
            Block =
            [
               new FilterOperatorInput()
                .Add<Order>(x => x.Product.Name, x => x.Items = ["mouse"])
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                .Add<Order>(x => x.Customer.FirstName, x => x.Items = ["Mary"])
                .Add<Order>(x => x.Customer.LastName, x => x.Items = ["Smith"]),
            ]
        },
        [
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate =new DateTime(2025, 3, 1)
                },
                Product = new Product()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new Customer()
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Smith"
                }
            }
        ]);

        Add(new FilterInput
        {
            Block =
            [
               new FilterOperatorInput()
                .Add<Order>(x => x.Product.Name, x => x.Items = ["keyboard"])
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan)
                .Add<Order>(x => x.OrderDate, x => x.Items = ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                .Add<Order>(x => x.Customer.FirstName, x => x.Items = ["Mary"])
                .Add<Order>(x => x.Customer.LastName, x => x.Items = ["Smith"]),
            ]
        },
        [
        ]);
    }
}


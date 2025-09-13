using DataDomain.Order;
using EFDeepQueryDynamicLinq;

namespace DeepQueryUnitTestDynamicLinq.TestData;

public class CustomerTestData : TheoryData<FilterInput, List<ExpectedData>>
{
    public CustomerTestData()
    {
        Add(new FilterInput
        {
            Block =
            [
                new FilterOperatorInput()
                .Add<Customer>(x => x.FirstName, x => x.Items = ["Joe"])
                .Add<Customer>(x => x.LastName, x => x.Items = ["Borg"]),
            ]
        },
        [
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 1,
                        OrderDate = new DateTime(2025, 1, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 1,
                            Name = "keyboard"
                        },
                    },
                    new Order()
                    {
                        Id = 2,
                        OrderDate = new DateTime(2025, 2, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 2,
                            Name = "mouse"
                        }
                    }
                ],
                Customer = new Customer()
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Borg"
                }
            }
        ]);

        Add(new FilterInput
        {
            Block =
            [
                new FilterOperatorInput()
                .Add<Customer>(x => x.FirstName, x => x.Items = ["Joe"])
                .Add<Customer>(x => x.LastName, x => x.Items = ["Borg"]),

                new FilterOperatorInput()
                .Add<Customer>(x => x.FirstName, x => x.Items = ["Mary"])
                .Add<Customer>(x => x.LastName, x => x.Items = ["Smith"]),
            ]
        },
        [
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 1,
                        OrderDate = new DateTime(2025, 1, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 1,
                            Name = "keyboard"
                        },
                    },
                    new Order()
                    {
                        Id = 2,
                        OrderDate = new DateTime(2025, 2, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 2,
                            Name = "mouse"
                        }
                    }
                ],
                Customer = new Customer()
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Borg"
                }
            },
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 3,
                        OrderDate = new DateTime(2025, 3, 1),
                        CustomerId = 2,
                        Product = new Product()
                        {
                            Id = 2,
                            Name = "mouse"
                        },
                    }
                ],
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
                .Add<Customer>(x => x.Orders.Count, x => x.Items = [1])
            ]
        },
        [
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 3,
                        OrderDate = new DateTime(2025, 3, 1),
                        CustomerId = 2,
                        Product = new Product()
                        {
                            Id = 2,
                            Name = "mouse"
                        },
                    }
                ],
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
                .Add<Customer>(x => x.Orders.Count, x => x.Items = [1], SearchOperator.GreaterThan)
            ]
        },
        [
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 1,
                        OrderDate = new DateTime(2025, 1, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 1,
                            Name = "keyboard"
                        },
                    },
                    new Order()
                    {
                        Id = 2,
                        OrderDate = new DateTime(2025, 2, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 2,
                            Name = "mouse"
                        }
                    }
                ],
                Customer = new Customer()
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Borg"
                }
            }
        ]);

        Add(new FilterInput
        {
            Block =
            [
                new FilterOperatorInput()
                .Add<Customer>(x => x.Orders.Count, x => x.Items = [1], SearchOperator.GreaterThanOrEquals)
            ]
        },
        [
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 1,
                        OrderDate = new DateTime(2025, 1, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 1,
                            Name = "keyboard"
                        },
                    },
                    new Order()
                    {
                        Id = 2,
                        OrderDate = new DateTime(2025, 2, 1),
                        CustomerId = 1,
                        Product = new Product()
                        {
                            Id = 2,
                            Name = "mouse"
                        }
                    }
                ],
                Customer = new Customer()
                {
                    Id = 1,
                    FirstName = "Joe",
                    LastName = "Borg"
                }
            },
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 3,
                        OrderDate = new DateTime(2025, 3, 1),
                        CustomerId = 2,
                        Product = new Product()
                        {
                            Id = 2,
                            Name = "mouse"
                        },
                    }
                ],
                Customer = new Customer()
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Smith"
                }
            }
            ]
        );
    }
}

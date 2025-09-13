using DataDomain.Order;
using EFDeepQueryDynamicLinq;

namespace DeepQueryUnitTestDynamicLinq.TestData;

public class OrderTestData : TheoryData<IFilterComponent, SortInput?, List<ExpectedData>>
{
    public OrderTestData()
    {
        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                FilterCondition.Create<Order>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                FilterCondition.Create<Order>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                FilterCondition.Create<Order>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"])
            ]
        },
        null,
        [
            new ExpectedData()
    {
        Order = new Order()
        {
            Id = 3,
            CustomerId = 2,
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

        Add(new FilterGroup
        {
            Operator = LogicalOperator.Or,
            Components =
            [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Components =
                    [
                         FilterCondition.Create<Order>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                         FilterCondition.Create<Order>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                         FilterCondition.Create<Order>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                         FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                         FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"])
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Components =
                    [
                        FilterCondition.Create<Order>(x => x.Id, [1, 3]),
                        FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Jo"], SearchOperator.Contains)
                    ]
                }
            ]
        },
        null,
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
            Id = 3,
            CustomerId = 2,
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

        Add(new FilterGroup
        {
            Operator = LogicalOperator.Or,
            Components =
            [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Components =
                    [
                         FilterCondition.Create<Order>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                         FilterCondition.Create<Order>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                         FilterCondition.Create<Order>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                         FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                         FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"])
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Components =
                    [
                        FilterCondition.Create<Order>(x => x.Id, [1, 3]),
                        FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Jo"], SearchOperator.Contains)
                    ]
                }
            ]
        },
        new SortInput().Add<Order>(x => x.Customer.FirstName, SortDirection.Desc),
        [
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 3,
                    CustomerId = 2,
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
            },
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
             }
        ]);

        Add(new FilterGroup
        {
            Operator = LogicalOperator.Or,
            Components = [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Components =
                    [
                         FilterCondition.Create<Order>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                         FilterCondition.Create<Order>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                         FilterCondition.Create<Order>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                         FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                         FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"]),
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Components =
                    [
                         FilterCondition.Create<Order>(x => x.Id, [1, 2]),
                         FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Jo"], SearchOperator.Contains),
                    ]
                }
            ]
        },
        null,
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
                    OrderDate = new DateTime(2025, 2, 1)
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

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                 FilterCondition.Create<Order>(x => x.Product.Name, ["mouse"]),
                 FilterCondition.Create<Order>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                 FilterCondition.Create<Order>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                 FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                 FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"]),
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 3,
                    CustomerId = 2,
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

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                 FilterCondition.Create<Order>(x => x.Product.Name, ["keyboard"]),
                 FilterCondition.Create<Order>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                 FilterCondition.Create<Order>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                 FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                 FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"]),
             ]
        },
        null,
        [
        ]);

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Components =
                    [
                        FilterCondition.Create<Order>(x => x.Product.Name, ["mouse", "monitor"]),
                        FilterCondition.Create<Order>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                        FilterCondition.Create<Order>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.Or,
                    Components =
                    [
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Components =
                            [
                                FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                                FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"]),
                            ]
                        },
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Components =
                            [
                                FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                                FilterCondition.Create<Order>(x => x.Customer.LastName, ["Black"]),
                            ]
                        }
                    ]
                }
            ]
        },
        new SortInput()
            .Add<Order>(x => x.Customer.FirstName, SortDirection.Asc)
            .Add<Order>(x => x.Customer.LastName, SortDirection.Asc),
        [
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 4,
                    CustomerId = 3,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                Product = new Product()
                {
                    Id = 3,
                    Name = "monitor"
                },
                Customer = new Customer()
                {
                    Id = 3,
                    FirstName = "Mary",
                    LastName = "Black"
                }
            },
            new ExpectedData()
            {
                Order = new Order()
                {
                    Id = 3,
                    CustomerId = 2,
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
    }
}


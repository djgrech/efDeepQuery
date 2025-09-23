using Common;
using DataDomain.Interfaces.Domain;

namespace DeepQueryUnitTestDynamicLinq.TestData;

public class OrderTestData : TheoryData<FilterGroup, SortInput?, List<ExpectedData>>
{
    public OrderTestData()
    {
        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Conditions =
            [
                FilterCondition.Create<OrderEntity>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Smith"])
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                Product = new ProductEntity()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new CustomerEntity()
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
            Groups =
            [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                         FilterCondition.Create<OrderEntity>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                         FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                         FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                         FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                         FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Smith"])
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                        FilterCondition.Create<OrderEntity>(x => x.Id, [1, 3]),
                        FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Jo"], SearchOperator.Contains)
                    ]
                }
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = new DateTime(2025, 1, 1)
                },
                 Product = new ProductEntity()
                 {
                     Id = 1,
                     Name = "keyboard"
                 },
                 Customer = new CustomerEntity()
                 {
                     Id = 1,
                     FirstName = "Joe",
                     LastName = "Borg"
                 }
             },
             new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                 Product = new ProductEntity()
                 {
                     Id = 2,
                     Name = "mouse"
                 },
                 Customer = new CustomerEntity()
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
            Groups =
            [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                         FilterCondition.Create<OrderEntity>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                         FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                         FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                         FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                         FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Smith"])
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                        FilterCondition.Create<OrderEntity>(x => x.Id, [1, 3]),
                        FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Jo"], SearchOperator.Contains)
                    ]
                }
            ]
        },
        new SortInput().Add<OrderEntity>(x => x.Customer.FirstName, SortDirection.Desc),
        [
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                Product = new ProductEntity()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new CustomerEntity()
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Smith"
                }
            },
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = new DateTime(2025, 1, 1)
                },
                 Product = new ProductEntity()
                 {
                     Id = 1,
                     Name = "keyboard"
                 },
                 Customer = new CustomerEntity()
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
            Groups = [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                         FilterCondition.Create<OrderEntity>(x => x.Product.Name, ["mou", "key"], SearchOperator.Contains),
                         FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                         FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                         FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                         FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Smith"]),
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                         FilterCondition.Create<OrderEntity>(x => x.Id, [1, 2]),
                         FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Jo"], SearchOperator.Contains),
                    ]
                }
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = new DateTime(2025, 1, 1)
                },
                 Product = new ProductEntity()
                 {
                     Id = 1,
                     Name = "keyboard"
                 },
                 Customer = new CustomerEntity()
                 {
                     Id = 1,
                     FirstName = "Joe",
                     LastName = "Borg"
                 }
             },
             new ExpectedData()
             {
                Order = new OrderEntity()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = new DateTime(2025, 2, 1)
                },
                 Product = new ProductEntity()
                 {
                     Id = 2,
                     Name = "mouse"
                 },
                 Customer = new CustomerEntity()
                 {
                     Id = 1,
                     FirstName = "Joe",
                     LastName = "Borg"
                 }
             },
             new ExpectedData()
             {
                Order = new OrderEntity()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                 Product = new ProductEntity()
                 {
                     Id = 2,
                     Name = "mouse"
                 },
                 Customer = new CustomerEntity()
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
            Conditions =
            [
                 FilterCondition.Create<OrderEntity>(x => x.Product.Name, ["mouse"]),
                 FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                 FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                 FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                 FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Smith"]),
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                Product = new ProductEntity()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new CustomerEntity()
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
            Conditions =
            [
                 FilterCondition.Create<OrderEntity>(x => x.Product.Name, ["keyboard"]),
                 FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                 FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan),
                 FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                 FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Smith"]),
             ]
        },
        null,
        [
        ]);

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Groups =
            [
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                        FilterCondition.Create<OrderEntity>(x => x.Product.Name, ["mouse", "monitor"]),
                        FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                        FilterCondition.Create<OrderEntity>(x => x.OrderDate, ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.Or,
                    Groups =
                    [
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Conditions =
                            [
                                FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                                FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Smith"]),
                            ]
                        },
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Conditions =
                            [
                                FilterCondition.Create<OrderEntity>(x => x.Customer.FirstName, ["Mary"]),
                                FilterCondition.Create<OrderEntity>(x => x.Customer.LastName, ["Black"]),
                            ]
                        }
                    ]
                }
            ]
        },
        new SortInput()
            .Add<OrderEntity>(x => x.Customer.FirstName, SortDirection.Asc)
            .Add<OrderEntity>(x => x.Customer.LastName, SortDirection.Asc),
        [
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 4,
                    CustomerId = 3,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                Product = new ProductEntity()
                {
                    Id = 3,
                    Name = "monitor"
                },
                Customer = new CustomerEntity()
                {
                    Id = 3,
                    FirstName = "Mary",
                    LastName = "Black"
                }
            },
            new ExpectedData()
            {
                Order = new OrderEntity()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = new DateTime(2025, 3, 1)
                },
                Product = new ProductEntity()
                {
                    Id = 2,
                    Name = "mouse"
                },
                Customer = new CustomerEntity()
                {
                    Id = 2,
                    FirstName = "Mary",
                    LastName = "Smith"
                }
            }
         ]);
    }
}


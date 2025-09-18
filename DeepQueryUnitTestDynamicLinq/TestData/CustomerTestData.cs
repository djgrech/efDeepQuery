using DataDomain.Order;
using EFDeepQueryDynamicLinq;

namespace DeepQueryUnitTestDynamicLinq.TestData;

public class CustomerTestData : TheoryData<FilterGroup, List<ExpectedData>>
{
    public CustomerTestData()
    {
        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Conditions =
            [
                FilterCondition.Create<Customer>(x => x.FirstName,["Joe"]),
                FilterCondition.Create<Customer>(x => x.LastName,["Borg"]),
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
                        FilterCondition.Create<Customer>(x => x.FirstName,["Joe"]),
                        FilterCondition.Create<Customer>(x => x.LastName,["Borg"]),
                    ]
                },
                new FilterGroup
                {
                    Operator = LogicalOperator.And,
                    Conditions =
                    [
                        FilterCondition.Create<Customer>(x => x.FirstName,["Mary"]),
                        FilterCondition.Create<Customer>(x => x.LastName,["Smith"]),
                    ]
                },
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

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Conditions =
            [
                FilterCondition.Create<Customer>(x => x.Orders.Count,[1])
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
            },
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 4,
                        OrderDate = new DateTime(2025, 3, 1),
                        CustomerId = 3,
                        Product = new Product()
                        {
                            Id = 3,
                            Name = "monitor"
                        },
                    }
                ],
                Customer = new Customer()
                {
                    Id = 3,
                    FirstName = "Mary",
                    LastName = "Black"
                }
            }
        ]);

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Conditions =
            [
                FilterCondition.Create<Customer>(x => x.Orders.Count,[1], SearchOperator.GreaterThan)
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

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Conditions =
            [
                FilterCondition.Create<Customer>(x => x.Orders.Count,[1], SearchOperator.GreaterThanOrEquals)
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
            },
            new ExpectedData()
            {
                Orders =
                [
                    new Order()
                    {
                        Id = 4,
                        OrderDate = new DateTime(2025, 3, 1),
                        CustomerId = 3,
                        Product = new Product()
                        {
                            Id = 3,
                            Name = "monitor"
                        },
                    }
                ],
                Customer = new Customer()
                {
                    Id = 3,
                    FirstName = "Mary",
                    LastName = "Black"
                }
            }
            ]
        );
    }
}

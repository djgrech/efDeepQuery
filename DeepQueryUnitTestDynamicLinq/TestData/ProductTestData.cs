using DataDomain.Order;
using EFDeepQueryDynamicLinq;

namespace DeepQueryUnitTestDynamicLinq.TestData;

public class ProductTestData : TheoryData<IFilterComponent, SortInput?, List<ExpectedData>>
{
    public ProductTestData()
    {
        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                FilterCondition.Create<Product>(x => x.Name, ["keyboard"])
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Product = new Product
                {
                    Id = 1,
                    Name = "keyboard",
                    Orders = [
                        new Order()
                        {
                            Id = 1,
                            OrderDate = new DateTime(2025,1,1),
                            CustomerId = 1,
                            Customer = new()
                            {
                                Id = 1,
                                FirstName = "Joe",
                                LastName = "Borg"
                            }
                        }
                    ]
                }
            }
        ]);

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                FilterCondition.Create<Product>(x => x.Name, ["mouse"])
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Product = new Product
                {
                    Id = 2,
                    Name = "mouse",
                    Orders = [
                        new Order()
                        {
                            Id = 2,
                            OrderDate = new DateTime(2025,2,1),
                            CustomerId = 1,
                            Customer = new()
                            {
                                Id = 1,
                                FirstName = "Joe",
                                LastName = "Borg"
                            }
                        },
                        new Order()
                        {
                            Id = 3,
                            OrderDate = new DateTime(2025,3,1),
                            CustomerId = 2,
                            Customer = new()
                            {
                                Id = 2,
                                FirstName = "Mary",
                                LastName = "Smith"
                            }
                        }
                    ]
                }
            }
        ]);

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                FilterCondition.Create<Product>(x => x.Name, ["mouse", "keyboard"])
            ]
        },
        new SortInput().Add<Product>(x => x.Name, SortDirection.Desc),
        [
            new ExpectedData()
            {
                Product = new Product
                {
                    Id = 2,
                    Name = "mouse",
                    Orders = [
                        new Order()
                        {
                            Id = 2,
                            OrderDate = new DateTime(2025,2,1),
                            CustomerId = 1,
                            Customer = new()
                            {
                                Id = 1,
                                FirstName = "Joe",
                                LastName = "Borg"
                            }
                        },
                        new Order()
                        {
                            Id = 3,
                            OrderDate = new DateTime(2025,3,1),
                            CustomerId = 2,
                            Customer = new()
                            {
                                Id = 2,
                                FirstName = "Mary",
                                LastName = "Smith"
                            }
                        }
                    ]
                }
            },
            new ExpectedData()
            {
                Product = new Product
                {
                    Id = 1,
                    Name = "keyboard",
                    Orders = [
                        new Order()
                        {
                            Id = 1,
                            OrderDate = new DateTime(2025,1,1),
                            CustomerId = 1,
                            Customer = new()
                            {
                                Id = 1,
                                FirstName = "Joe",
                                LastName = "Borg"
                            }
                        }
                    ]
                }
            }
        ]);

        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            [
                FilterCondition.Create<Product>(x => x.Name, ["mouse", "keyboard"])
            ]
        },
        new SortInput().Add("Name"),
        [
            new ExpectedData()
            {
                Product = new Product
                {
                    Id = 1,
                    Name = "keyboard",
                    Orders = [
                        new Order()
                        {
                            Id = 1,
                            OrderDate = new DateTime(2025,1,1),
                            CustomerId = 1,
                            Customer = new()
                            {
                                Id = 1,
                                FirstName = "Joe",
                                LastName = "Borg"
                            }
                        }
                    ]
                }
            },
            new ExpectedData()
            {
                Product = new Product
                {
                    Id = 2,
                    Name = "mouse",
                    Orders = [
                        new Order()
                        {
                            Id = 2,
                            OrderDate = new DateTime(2025,2,1),
                            CustomerId = 1,
                            Customer = new()
                            {
                                Id = 1,
                                FirstName = "Joe",
                                LastName = "Borg"
                            }
                        },
                        new Order()
                        {
                            Id = 3,
                            OrderDate = new DateTime(2025,3,1),
                            CustomerId = 2,
                            Customer = new()
                            {
                                Id = 2,
                                FirstName = "Mary",
                                LastName = "Smith"
                            }
                        }
                    ]
                }
            }
        ]);
    }
}

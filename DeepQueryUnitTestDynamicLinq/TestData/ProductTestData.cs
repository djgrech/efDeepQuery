using Common;
using DataDomain.Interfaces.Domain;

namespace DeepQueryUnitTestDynamicLinq.TestData;

public class ProductTestData1 : TheoryData<FilterGroup, SortInput?, List<ExpectedData>>
{
    public ProductTestData1()
    {
        Add(new FilterGroup
        {
            Operator = LogicalOperator.And,
            Conditions =
            [
                FilterCondition.Create<ProductEntity>(x => x.Name, ["keyboard"])
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Product = new ProductEntity
                {
                    Id = 1,
                    Name = "keyboard",
                    Orders = [
                        new OrderEntity()
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
            Conditions =
            [
                FilterCondition.Create<ProductEntity>(x => x.Name, ["mouse"])
            ]
        },
        null,
        [
            new ExpectedData()
            {
                Product = new ProductEntity
                {
                    Id = 2,
                    Name = "mouse",
                    Orders = [
                        new OrderEntity()
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
                        new OrderEntity()
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
            Conditions =
            [
                FilterCondition.Create<ProductEntity>(x => x.Name, ["mouse", "keyboard"])
            ]
        },
        new SortInput().Add<ProductEntity>(x => x.Name, SortDirection.Desc),
        [
            new ExpectedData()
            {
                Product = new ProductEntity
                {
                    Id = 2,
                    Name = "mouse",
                    Orders = [
                        new OrderEntity()
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
                        new OrderEntity()
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
                Product = new ProductEntity
                {
                    Id = 1,
                    Name = "keyboard",
                    Orders = [
                        new OrderEntity()
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
            Conditions =
            [
                FilterCondition.Create<ProductEntity>(x => x.Name, ["mouse", "keyboard"])
            ]
        },
        new SortInput().Add("Name"),
        [
            new ExpectedData()
            {
                Product = new ProductEntity
                {
                    Id = 1,
                    Name = "keyboard",
                    Orders = [
                        new OrderEntity()
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
                Product = new ProductEntity
                {
                    Id = 2,
                    Name = "mouse",
                    Orders = [
                        new OrderEntity()
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
                        new OrderEntity()
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

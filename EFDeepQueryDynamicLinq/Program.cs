using Common;
using DataAccess;
using DataDomain.Interfaces.Domain;
using EFDeepQueryDynamicLinq;

//Test2();
Test3();


void Test2()
{
    var filter = new FilterGroup
    {
        Operator = LogicalOperator.And,
        Groups =
        {
            new FilterGroup
            {
                Operator = LogicalOperator.And,
                Conditions =
                {
                    FilterCondition.Create<Order>(x => x.Product.Name, ["mouse", "monitor"]),
                    FilterCondition.Create<Order>(x => x.OrderDate,  ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                    FilterCondition.Create<Order>(x => x.OrderDate,  ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
                }
            },
            new FilterGroup
            {
                Operator = LogicalOperator.Or,
                Groups =
                    {
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Conditions =
                            {
                                FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                                FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"])
                            }
                        },
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Conditions =
                            {
                                FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                                FilterCondition.Create<Order>(x => x.Customer.LastName, ["Black"])
                            }
                        }
                    }
            }
        }
    };

    var sortInput = new SortInput
{
    { "Customer.FirstName", SortDirection.Desc },
    { "OrderDate", SortDirection.Asc }
};

    var context = new ApplicationContext();

    var efFilterTranslator = new EFFilterTranslator();
    var query = context.Set<Order>().AsQueryable();

    query = efFilterTranslator.BuildQuery(query, filter, sortInput);

    var result = query.ToList();

    result = null;
}


void Test3()
{
    var filter = new FilterGroup
    {
        Operator = LogicalOperator.And,
        Groups =
        {
            new FilterGroup
            {
                Operator = LogicalOperator.Or,
                Groups =
                    {
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Conditions =
                            {
                                FilterCondition.Create<Customer>(x => x.FirstName, ["Mary"]),
                                FilterCondition.Create<Customer>(x => x.LastName, ["Smith"])
                            }
                        },
                        new FilterGroup
                        {
                            Operator = LogicalOperator.And,
                            Conditions =
                            {
                                FilterCondition.Create<Customer>(x => x.FirstName, ["Mary"]),
                                FilterCondition.Create<Customer>(x => x.LastName, ["Black"])
                            }
                        }
                    }
            }
        }
    };

    var context = new ApplicationContext();

    var efFilterTranslator = new EFFilterTranslator();
    var query = context.Set<Customer>().AsQueryable();

    query = efFilterTranslator.BuildQuery(query, filter);

    var result = query.ToList();

    result = null;
}

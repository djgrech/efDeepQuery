using DataAccess;
using DataDomain.Order;
using EFDeepQueryDynamicLinq;

var filter = new FilterGroup
{
    Operator = LogicalOperator.And,
    Components =
    {
        new FilterGroup
        {
            Operator = LogicalOperator.And,
            Components =
            {
                FilterCondition.Create<Order>(x => x.Product.Name, ["mouse", "monitor"]),
                FilterCondition.Create<Order>(x => x.OrderDate,  ["2024-02-15T00:00:00Z"], SearchOperator.GreaterThan),
                FilterCondition.Create<Order>(x => x.OrderDate,  ["2025-03-15T00:00:00Z"], SearchOperator.LessThan)
            }
        },
        new FilterGroup
        {
            Operator = LogicalOperator.Or,
            Components =
                {
                    new FilterGroup
                    {
                        Operator = LogicalOperator.And,
                        Components =
                        {
                            FilterCondition.Create<Order>(x => x.Customer.FirstName, ["Mary"]),
                            FilterCondition.Create<Order>(x => x.Customer.LastName, ["Smith"])
                        }
                    },
                    new FilterGroup
                    {
                        Operator = LogicalOperator.And,
                        Components =
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

result = null;

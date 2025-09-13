using DataAccess;
using DataDomain.Order;
using EFDeepQueryDynamicLinq;

var filterInput = new FilterInput
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
};

var sortInput = new SortInput
{
    { "Customer.FirstName", SortDirection.Desc },
    { "OrderDate", SortDirection.Asc }
};

var context = new ApplicationContext();

var efFilterTranslator = new EFFilterTranslator();
var query = context.Set<Order>().AsQueryable();

query = efFilterTranslator.Build(query, filterInput, sortInput);

var result = query.ToList();

result = null;

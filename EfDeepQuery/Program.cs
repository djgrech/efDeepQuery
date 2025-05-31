using DataAccess;
using DataDomain;


var context = new ApplicationContext();

var dataService = new DataService();

var orders = context.Orders.ToList();
var products = context.Products.ToList();
var customers = context.Customers.ToList();

var r1 = await dataService.Query<Customer>(context, "Orders", "Orders.Product.Name", ["mouse"], new List<Sort>()
{
    new Sort()
    {
        Property = "FirstName",
        Direction = Direction.Descending
    }
});


var r2 = await dataService.Query<Order>(context, "Customer", "Customer.FirstName", ["Joe"], new List<Sort>()
{
    new Sort()
    {
        Property = "OrderDate",
        Direction = Direction.Descending
    }
});

var list1 = context.Brands.Where(x => x.Name == "brand-1").ToList();

var list2 = context.Organizations.Where(x => x.Brands.Select(x => x.Id).Contains("brand-3")).ToList();
list1 = null;




var organizations = await dataService.Query<Organization>(context, "Brands.Name", "Brands.Name", ["brand-1", "brand-3"]);




var brands = await dataService.Query<Brand>(context, "Organization.Name", "Organization.Name", ["organization-1"]);


var blogs1 = await dataService.Query<Blog>(context, "Posts.User.Name", "Posts.User.Name", ["joe"]);

Print(blogs1);

var blogs2 = await dataService.Query<Blog>(context, "Posts.User.Name", "Posts.User.Name", ["mary"]);
Print(blogs2);

void Print(List<Blog> blogs)
{
    foreach (var r in blogs)
    {
        Console.WriteLine($"blog: {r.Name}");
        foreach (var post in r.Posts)
            Console.WriteLine($"Post {post.Title}, userId {post.UserId}, userName: {post.User.Name}");
    }

    Console.WriteLine();
}


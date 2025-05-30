using DataAccess;
using DataDomain;


var context = new ApplicationContext();

var list1 = context.Brands.Where(x => x.Name == "brand-1").ToList();

var list2 = context.Organizations.Where(x => x.Brands.Select(x => x.Id).Contains("brand-3")).ToList();
list1 = null;


var dataService = new DataService();

var organizations = await dataService.Query<Organization>(context, "Brands.Name", "Brands.Name", ["brand-1", "brand-2"]);

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


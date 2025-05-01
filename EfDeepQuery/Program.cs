using DataAccess;
using DataDomain;


var context = new ApplicationContext();


var dataService = new DataService();

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


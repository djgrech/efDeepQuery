using DataDomain;

namespace DataAccess;

public static class DataSeeding
{
    public static List<Blog> Blogs =
    [
        new Blog()
    {
        Id = 1,
        Name = "blog1"
    },
    new Blog()
    {
        Id = 2,
        Name = "blog2"
    }
    ];

    public static List<Post> Posts =
    [
        new Post()
    {
        Id = 1,
        BlogId = 1,
        Title = "post 1",
        UserId = 1
    },
    new Post()
    {
        Id = 2,
       BlogId = 1,
        Title = "post 2",
        UserId = 2
    },
    new Post()
    {
        Id = 3,
        BlogId = 2,
        Title = "post 3",
        UserId = 1
    }
    ];

    public static List<User> Users =
    [
        new User()
    {
        Id=1,
       Name= "joe",
    },
    new User()
    {
        Id=2,
        Name= "mary",
    }
    ];
}

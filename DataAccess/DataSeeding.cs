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


    public static List<Brand> Brands =
    [
        new Brand()
        {
            Id = "brand-1",
            Name = "brand-1",
            OrganizationId = "organization-1"
        },
        new Brand()
        {
            Id = "brand-2",
            Name = "brand-2",
            OrganizationId = "organization-1"
        },
        new Brand()
        {
            Id = "brand-3",
            Name = "brand-3",
            OrganizationId = "organization-2"
        }
    ];

    public static List<Organization> Organizations =
    [
        new Organization()
        {
            Id = "organization-1",
            Name = "organization-1"
        },
        new Organization()
        {
            Id = "organization-2",
            Name = "organization-2"
        },
    ];
}

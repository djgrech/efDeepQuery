using DataDomain;
using DataDomain.Interfaces.Domain;

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




    public static List<ProductEntity> Products =
    [
        new ProductEntity()
        {
            Id = 1,
            Name = "keyboard"
        },
        new ProductEntity()
        {
            Id = 2,
            Name = "mouse"
        },
        new ProductEntity()
        {
            Id = 3,
            Name = "monitor"
        }
    ];


    public static List<OrderEntity> Orders =
    [
        new OrderEntity()
        {
            Id = 1,
            CustomerId = 1,
            OrderDate = new DateTime(2025,1,1),
            ProductId = 1
        },
        new OrderEntity()
        {
            Id = 2,
            CustomerId = 1,
            OrderDate = new DateTime(2025,2,1),
            ProductId = 2
        },
        new OrderEntity()
        {
            Id = 3,
            CustomerId = 2,
            OrderDate = new DateTime(2025,3,1),
            ProductId = 2
        },
        new OrderEntity()
        {
            Id = 4,
            CustomerId = 3,
            OrderDate = new DateTime(2025,3,1),
            ProductId = 3
        }
    ];

    public static List<CustomerEntity> Customers =
    [
        new CustomerEntity()
        {
            Id = 1,
            FirstName = "Joe",
            LastName = "Borg"
        },
        new CustomerEntity()
        {
            Id = 2,
            FirstName = "Mary",
            LastName = "Smith"
        },
        new CustomerEntity()
        {
            Id = 3,
            FirstName = "Mary",
            LastName = "Black"
        },
        new CustomerEntity()
        {
            Id = 4,
            FirstName = "Peter",
            LastName = "Jones"
        }
    ];
}

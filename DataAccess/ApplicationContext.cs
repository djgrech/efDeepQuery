using DataDomain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationContext : DbContext
{
    public ApplicationContext() 
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=localhost;Database=blogDb;User Id=sa;Password=pasword;TrustServerCertificate=True");
        //optionsBuilder.UseInMemoryDatabase("testDatabase");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Blog>(b =>
        {
            b.HasData(DataSeeding.Blogs);
        });

        modelBuilder.Entity<Post>(b =>
        {
            b.HasData(DataSeeding.Posts);
        });

        modelBuilder.Entity<User>(b =>
        {
            b.HasData(DataSeeding.Users);
        });
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
}

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

using DataDomain;
using DataDomain.Interfaces.Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                //.UseInMemoryDatabase("testDatabase")
                .UseSqlServer("Server=.\\SQLExpress;Database=blogDb;Trusted_Connection=True;TrustServerCertificate=True")
                .UseLazyLoadingProxies() // may impact performance
                ;
        }
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

        modelBuilder.Entity<Brand>(b =>
        {
            b.HasData(DataSeeding.Brands);
        });

        modelBuilder.Entity<Organization>(b =>
        {
            b.HasData(DataSeeding.Organizations);
        });


        // product

        modelBuilder.Entity<Product>(b =>
        {
            b.HasData(DataSeeding.Products);
        });

        modelBuilder.Entity<Customer>(b =>
        {
            b.HasData(DataSeeding.Customers);
        });

        modelBuilder.Entity<Order>(b =>
        {
            b.HasData(DataSeeding.Orders);
        });


        modelBuilder.Entity<Product>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductId);

        modelBuilder.Entity<Customer>()
            .HasMany(c => c.Orders)  // Add this relationship
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }

}

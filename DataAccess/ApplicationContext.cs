using DataDomain;
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
                .UseSqlServer("Server=.\\SQLExpress;Database=orders;Trusted_Connection=True;TrustServerCertificate=True")
                .UseLazyLoadingProxies() // may impact performance
                ;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // product

        modelBuilder.Entity<ProductEntity>(b =>
        {
            b.HasData(DataSeeding.Products);
        });

        modelBuilder.Entity<CustomerEntity>(b =>
        {
            b.HasData(DataSeeding.Customers);
        });

        modelBuilder.Entity<OrderEntity>(b =>
        {
            b.HasData(DataSeeding.Orders);
        });


        modelBuilder.Entity<ProductEntity>()
            .HasMany(p => p.Orders)
            .WithOne(o => o.Product)
            .HasForeignKey(o => o.ProductId);

        modelBuilder.Entity<CustomerEntity>()
            .HasMany(c => c.Orders)  // Add this relationship
            .WithOne(o => o.Customer)
            .HasForeignKey(o => o.CustomerId);
    }

    public virtual DbSet<ProductEntity> Products { get; set; }
    public virtual DbSet<OrderEntity> Orders { get; set; }
    public virtual DbSet<CustomerEntity> Customers { get; set; }

}

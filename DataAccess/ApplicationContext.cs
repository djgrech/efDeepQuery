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
        optionsBuilder
            //.UseInMemoryDatabase("testDatabase")
            .UseSqlServer("Server=.\\SQLExpress;Database=blogDb;Trusted_Connection=True;TrustServerCertificate=True")
            .UseLazyLoadingProxies() // may impact performance
            ;
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

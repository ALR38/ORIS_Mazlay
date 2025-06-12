// Infrastructure/Data/ApplicationDbContext.cs
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // DbSets
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product>  Products   => Set<Product>();
    public DbSet<Order>    Orders     => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // таблицы — явные имена
        builder.Entity<Category>().ToTable("Categories");
        builder.Entity<Product>().ToTable("Products");
        builder.Entity<Order>().ToTable("Orders");
        builder.Entity<OrderItem>().ToTable("OrderItems");

        // decimal точность
        builder.Entity<Product>()
               .Property(p => p.Price)
               .HasColumnType("decimal(18,2)");

        builder.Entity<OrderItem>()
               .Property(p => p.UnitPrice)
               .HasColumnType("decimal(18,2)");

        // связи "один-ко-многим"
        builder.Entity<Product>()
               .HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId);

        builder.Entity<Order>()
               .HasOne(o => o.User)
               .WithMany(u => u.Orders)
               .HasForeignKey(o => o.ApplicationUserId);

        builder.Entity<OrderItem>()
               .HasOne(i => i.Order)
               .WithMany(o => o.Items)
               .HasForeignKey(i => i.OrderId);

        builder.Entity<OrderItem>()
               .HasOne(i => i.Product)
               .WithMany(p => p.OrderItems)
               .HasForeignKey(i => i.ProductId);
    }
}

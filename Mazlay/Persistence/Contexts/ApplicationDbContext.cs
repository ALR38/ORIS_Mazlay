using System;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

/// <summary>
///  Db-контекст с Identity + бизнес-сущностями
/// </summary>
public sealed class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Category>  Categories  => Set<Category>();
    public DbSet<Product>   Products    => Set<Product>();
    public DbSet<Order>     Orders      => Set<Order>();
    public DbSet<OrderItem> OrderItems  => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        /* ---------- Category ---------- */
        b.Entity<Category>(e =>
        {
            e.Property(c => c.Name)
             .IsRequired()
             .HasMaxLength(128);

            // обратная связь:  Category.Products
            e.HasMany(c => c.Products)
             .WithOne(p => p.Category)
             .HasForeignKey(p => p.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        /* ---------- Product ---------- */
        b.Entity<Product>(e =>
        {
            e.Property(p => p.Name)
             .IsRequired()
             .HasMaxLength(256);

            e.Property(p => p.Price)
             .HasColumnType("decimal(10,2)");

            // FK описан уже в Category section → ничего «CategoryId1» не появится
        });

        /* ---------- Order ---------- */
        b.Entity<Order>(e =>
        {
            e.Property(o => o.CreatedUtc)
             .HasColumnType("timestamp with time zone");

            e.Property(o => o.Total)
             .HasColumnType("decimal(10,2)");

            e.HasMany(o => o.Items)
             .WithOne(i => i.Order)
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        /* ---------- OrderItem ---------- */
        b.Entity<OrderItem>(e =>
        {
            e.Property(i => i.Price)
             .HasColumnType("decimal(10,2)");

            e.HasOne(i => i.Product)
             .WithMany()                    // у Product нет коллекции OrderItems
             .HasForeignKey(i => i.ProductId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}

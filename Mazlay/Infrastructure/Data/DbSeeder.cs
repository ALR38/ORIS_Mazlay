using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        if (!await db.Categories.AnyAsync())
        {
            db.Categories.Add(new Category { Name = "General" });
            await db.SaveChangesAsync();
        }

        if (!await db.Products.AnyAsync())
        {
            int catId = await db.Categories.Select(c => c.Id).FirstAsync();

            db.Products.AddRange(
                new Product { Name = "Nunc Neque Eros",  Price = 120, ImageMain = "product1.jpg",  ImageAlt = "product2.jpg",  CategoryId = catId },
                new Product { Name = "Mauris Vel Tellus",Price = 180, ImageMain = "product1.jpg",  ImageAlt = "product2.jpg",  CategoryId = catId },
                new Product { Name = "Lorem Ipsum Lec",  Price = 110, ImageMain = "product3.jpg",  ImageAlt = "product4.jpg",  CategoryId = catId },
                new Product { Name = "Proin Lectus Ipsum",Price=190, ImageMain = "product5.jpg",  ImageAlt = "product6.jpg",  CategoryId = catId },
                new Product { Name = "Ras Neque Metus",   Price = 220, ImageMain = "product7.jpg",  ImageAlt = "product8.jpg",  CategoryId = catId },
                new Product { Name = "Mauris Vel Tellus",Price = 180, ImageMain = "product9.jpg",  ImageAlt = "product10.jpg", CategoryId = catId },
                new Product { Name = "Donec Non Est",     Price = 120, ImageMain = "product11.jpg", ImageAlt = "product12.jpg", CategoryId = catId },
                new Product { Name = "Cas Meque Metus",   Price = 180, ImageMain = "product1.jpg",  ImageAlt = "product2.jpg",  CategoryId = catId }
            );
            await db.SaveChangesAsync();
        }
    }
}
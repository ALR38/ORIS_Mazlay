using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;         
using System.Threading.Tasks;

namespace Infrastructure.Services;

/// <inheritdoc/>
public class ProductService : IProductService
{
    private readonly ApplicationDbContext _db;
    public ProductService(ApplicationDbContext db) => _db = db;

    /* 1. последние N товаров */
    public async Task<IReadOnlyList<Product>> GetLatestAsync(int count = 8) =>
        await _db.Products.OrderByDescending(p => p.Id)
                          .Take(count)
                          .ToListAsync();

    /* 2. конкретный товар  */
    public Task<Product?> GetByIdAsync(int id) =>
        _db.Products.Include(p => p.Category)
                    .FirstOrDefaultAsync(p => p.Id == id);

    /* 3. поиск + пагинация */
    public async Task<(IReadOnlyList<Product>, int)> SearchAsync(
        int page, int pageSize, int? categoryId, string? query)
    {
        var q = _db.Products.AsQueryable();

        if (categoryId.HasValue)
            q = q.Where(p => p.CategoryId == categoryId);

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(p => p.Name.Contains(query));

        var total = await q.CountAsync();
        var items = await q.OrderBy(p => p.Id)
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();

        return (items, total);
    }

    /* 4. ← предыдущий товар */
    public async Task<int?> GetPrevIdAsync(int currentId) =>
        await _db.Products
                 .Where(p => p.Id < currentId)
                 .OrderByDescending(p => p.Id)
                 .Select(p => (int?)p.Id)
                 .FirstOrDefaultAsync();

    /* 5. → следующий товар */
    public async Task<int?> GetNextIdAsync(int currentId) =>
        await _db.Products
                 .Where(p => p.Id > currentId)
                 .OrderBy(p => p.Id)
                 .Select(p => (int?)p.Id)
                 .FirstOrDefaultAsync();
}

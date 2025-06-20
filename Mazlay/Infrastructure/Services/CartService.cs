using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Data;
using Infrastructure.Mongo;
using MongoDB.Driver;

namespace Infrastructure.Services;

public sealed class CartService : ICartService
{
    private readonly IMongoCollection<CartDoc> _collection;
    private readonly ApplicationDbContext _db;

    public CartService(MongoDbContext mongo, ApplicationDbContext db)
    {
        _collection = mongo.Db.GetCollection<CartDoc>("Cart");
        _db = db;
    }

    public async Task<IReadOnlyList<CartLineDto>> GetAsync(Guid userId)
    {
        var doc = await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync();
        return doc?.Lines.Select(ToDto).ToList() ?? new List<CartLineDto>();
    }

    public async Task AddAsync(Guid userId, int productId, int qty = 1)
    {
        var prod = await _db.Products.FindAsync(productId);
        if (prod is null) return;

        var doc = await _collection.Find(x => x.UserId == userId).FirstOrDefaultAsync()
                  ?? new CartDoc { UserId = userId };

        var line = doc.Lines.FirstOrDefault(l => l.ProductId == productId);
        if (line is null)
        {
            doc.Lines.Add(new CartDoc.Line
            {
                ProductId = prod.Id,
                Name = prod.Name,
                Price = prod.Price,
                Qty = qty
            });
        }
        else
        {
            line.Qty += qty;
        }

        await _collection.ReplaceOneAsync(x => x.UserId == userId,
            doc,
            new ReplaceOptions { IsUpsert = true });
    }

    public Task RemoveAsync(Guid userId, int productId) =>
        _collection.UpdateOneAsync(
            x => x.UserId == userId,
            Builders<CartDoc>.Update.PullFilter(d => d.Lines,
                l => l.ProductId == productId));

    public Task ClearAsync(Guid userId) =>
        _collection.DeleteOneAsync(x => x.UserId == userId);

    /*──────── helpers ────────*/
    private static CartLineDto ToDto(CartDoc.Line l)
        => new(l.ProductId, l.Name, l.Price, l.Qty);

    /* внутренний документ */
    private sealed class CartDoc
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Line> Lines { get; set; } = new();

        public sealed class Line
        {
            public int ProductId { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Qty { get; set; }
        }
    }
}

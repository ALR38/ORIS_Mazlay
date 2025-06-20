using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Mongo;
using MongoDB.Driver;

namespace Infrastructure.Services;

public sealed class WishlistService : IWishlistService
{
    private sealed class Doc
    {
        public Guid       Id     { get; set; }
        public Guid       UserId { get; set; }
        public List<int>  Items  { get; set; } = new();
    }

    private readonly IMongoCollection<Doc> _collection;

    public WishlistService(MongoDbContext mongo) =>
        _collection = mongo.Db.GetCollection<Doc>("Wishlist");

    public async Task<IList<int>> GetAsync(Guid userId) =>
        (await _collection.Find(d => d.UserId == userId)
            .FirstOrDefaultAsync())?.Items ?? new List<int>();

    public async Task ToggleAsync(Guid userId, int productId)
    {
        var doc = await _collection.Find(d => d.UserId == userId)
            .FirstOrDefaultAsync();

        if (doc == null)
        {
            await _collection.InsertOneAsync(new Doc
            {
                UserId = userId,
                Items  = new() { productId }
            });
            return;
        }

        if (doc.Items.Contains(productId)) doc.Items.Remove(productId);
        else                                doc.Items.Add(productId);

        await _collection.ReplaceOneAsync(d => d.Id == doc.Id, doc);
    }
}
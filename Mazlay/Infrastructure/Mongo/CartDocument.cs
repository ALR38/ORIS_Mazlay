// Infrastructure/Mongo/CartDocument.cs

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class CartDocument
{
    [BsonId] public ObjectId Id { get; set; }
    public Guid UserId { get; set; }

    public IList<CartLine> Lines { get; set; } = new List<CartLine>();

    public class CartLine
    {
        public int     ProductId { get; set; }   // ← int!
        public string  Name      { get; set; } = string.Empty;
        public decimal Price     { get; set; }
        public int     Qty       { get; set; }
    }
}

// Infrastructure/Mongo/WishlistDocument.cs
public class WishlistDocument
{
    [BsonId] public ObjectId Id { get; set; }
    public Guid UserId { get; set; }
    public IList<int> ProductIds { get; set; } = new List<int>();   // ← int!
}
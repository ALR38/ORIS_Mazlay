using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Infrastructure.Mongo;

/// <summary>Документ корзины в MongoDB.</summary>
public class CartDocument
{
    [BsonId] public ObjectId Id { get; set; }

    /// <summary>Guid пользователя (тот же, что в Identity).</summary>
    public Guid UserId { get; set; }

    public IList<CartLine> Lines { get; set; } = new List<CartLine>();

    public class CartLine
    {
        public Guid    ProductId { get; set; }
        public string  Name      { get; set; } = string.Empty;
        public decimal Price     { get; set; }
        public int     Qty       { get; set; }
    }
}
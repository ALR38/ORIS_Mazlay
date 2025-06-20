using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Infrastructure.Mongo;

/// <summary>Документ wish‑листа в MongoDB.</summary>
public class WishlistDocument
{
    [BsonId] public ObjectId Id { get; set; }

    public Guid UserId { get; set; }
    
    public IList<int> ProductIds { get; set; } = new List<int>();
}
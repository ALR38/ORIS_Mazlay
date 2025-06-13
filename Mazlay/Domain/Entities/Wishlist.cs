using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Mazlay.Domain.Entities
{
    /// <summary>Документ wish-листа в MongoDB.</summary>
    public class Wishlist
    {
        [BsonId] public ObjectId Id { get; set; }

        public Guid UserId { get; set; }

        public List<Guid> ProductIds { get; set; } = new();
    }
}
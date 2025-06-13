using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.Entities
{
    /// <summary>Документ корзины в MongoDB.</summary>
    public class Cart
    {
        [BsonId] public ObjectId Id { get; set; }

        /// <summary>Guid пользователя (тот же, что в Identity).</summary>
        public Guid UserId { get; set; }

        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public Guid ProductId { get; set; }
        public int  Quantity  { get; set; }
    }
}
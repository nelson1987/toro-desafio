using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Toro.Core.Entities
{
    public class Trend
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string Symbol { get; set; }
        public decimal CurrentPrice { get; set; }
        public int Buys { get; set; }
    }
}

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShortcutsBotHost.MongoModels
{
    public class MongoItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}
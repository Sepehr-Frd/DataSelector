using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmazonMockup.Model.Models;

public class BaseMongoDbDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}


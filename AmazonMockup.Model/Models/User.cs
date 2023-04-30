using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmazonMockup.Model.Models;

public class User : BaseMongoDbDocument
{
    public string? Username { get; set; }

    public string? Password { get; set; }
}


using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AmazonMockup.Model.Models;

public class Person : BaseMongoDbDocument
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; }
}


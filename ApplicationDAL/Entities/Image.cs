using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Image
{
    [BsonRepresentation(BsonType.ObjectId)]
    public int Id { get; set; }
    public string Url { get; set; }
}
using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace ApplicationDAL.Entities;

public class Host
{
    [BsonId]
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    
    [RestrictUpdate]
    public int NumberOfListings { get; set; }
    
    [RestrictUpdate]
    public int NumberOfReviews { get; set; }
    
    public double Rating { get; set; }

    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> ListingsIds { get; set; } = new();
    
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.DateTime)]
    [BsonSerializer(typeof(DateTimeSerializer))]
    [RestrictUpdate]
    public DateTime LastAccess { get; set; }
}
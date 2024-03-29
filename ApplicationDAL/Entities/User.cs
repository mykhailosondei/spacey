using ApplicationCommon.Structs;
using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace ApplicationDAL.Entities;

public class User
{
    [BsonId]
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    [RestrictUpdate]
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    [RestrictUpdate]
    public DateTime CreatedAt { get; set; }
    
    [RestrictUpdate]
    public string PasswordHash { get; set; }
    
    public Address Address { get; set; }
    
    public string Description { get; set; }
    
    public Image Avatar { get; set; }
    
    [RestrictUpdate]
    public Host Host { get; set; }
    
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> BookingsIds { get; set; }
    
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public List<Guid> ReviewsIds { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public List<Guid> LikedListingsIds { get; set; }
    
    [BsonIgnoreIfNull]
    [BsonRepresentation(BsonType.DateTime)]
    [BsonSerializer(typeof(DateTimeSerializer))]
    [RestrictUpdate]
    public DateTime LastAccess { get; set; }
}
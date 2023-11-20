using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
    
    public DateTime CreatedAt { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string Address { get; set; }
    
    public string Description { get; set; }
    
    public Image Avatar { get; set; }
    
    public Host Host { get; set; }
    
    [RestrictUpdate]
    public List<Guid> BookingsIds { get; set; }
    
    [RestrictUpdate]
    public List<Guid> ReviewsIds { get; set; }
}
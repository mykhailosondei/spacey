using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Conversation
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid User1Id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid HostId { get; set; }
    
    public List<Message> Messages { get; set; } = new();
    
    [BsonRepresentation(BsonType.String)]
    public Guid BookingId { get; set; }
    
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
}

public class Message
{
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonRepresentation(BsonType.String)]
    public Guid? UserId { get; set; }
    
    public Guid? HostId { get; set; }
    
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
}
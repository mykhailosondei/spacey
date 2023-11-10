using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Host
{
    [BsonId]
    [RestrictUpdate]
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    [RestrictUpdate]
    public Guid UserId { get; set; }
    
    [RestrictUpdate]
    public int NumberOfListings { get; set; }
    
    [RestrictUpdate]
    public int NumberOfReviews { get; set; }
    
    public double Rating { get; set; }
    
    [RestrictUpdate]
    public List<Guid> ListingsIds { get; set; }
}
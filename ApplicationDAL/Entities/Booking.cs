using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Booking
{
    [BsonId]
    [BsonIgnoreIfNull]
    public Guid Id { get; set; }
    
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CheckIn { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CheckOut { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid ListingId { get; set; }
    
    [BsonIgnoreIfNull]
    public Guid ReviewId { get; set; }
    
    [BsonIgnoreIfNull]
    public int NumberOfGuests { get; set; }
    
    [BsonIgnoreIfNull]
    public int TotalPrice { get; set; }
    
    [BsonIgnoreIfNull]
    public bool IsCancelled { get; set; }
    
}
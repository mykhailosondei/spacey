using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Booking
{
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    
    public DateTime CheckIn { get; set; }
    
    public DateTime CheckOut { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid ListingId { get; set; }
    
    public Review Review { get; set; }
    
    public int NumberOfGuests { get; set; }
    
    public int TotalPrice { get; set; }
    
    public bool IsCancelled { get; set; }
    
}
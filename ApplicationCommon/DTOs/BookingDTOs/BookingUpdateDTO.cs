using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.BookingDTOs;

public class BookingUpdateDTO
{
    [BsonId]
    [BsonIgnoreIfNull]
    public Guid? Id { get; set; }
    
    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CheckIn { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime CheckOut { get; set; }
    
    public Guid UserId { get; set; }
    
    public Guid ListingId { get; set; }
    
    [BsonIgnoreIfNull]
    public int NumberOfGuests { get; set; }
}
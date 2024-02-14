using ApplicationCommon.DTOs.Review;
using ApplicationCommon.Enums;
using ApplicationCommon.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.BookingDTOs;

public class BookingDTO : ILastAccessible
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
    public ReviewDTO? Review { get; set; }
    
    [BsonIgnoreIfNull]
    public int NumberOfGuests { get; set; }
    
    [BsonIgnoreIfNull]
    public int TotalPrice { get; set; }
    
    [BsonIgnoreIfNull]
    public BookingStatus Status { get; set; }
    
    [BsonIgnoreIfNull]
    public DateTime LastAccess { get; set; }
}
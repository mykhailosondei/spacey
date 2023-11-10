using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace ApplicationDAL.Entities;

public class Booking
{
    [BsonIgnoreIfNull]
    [RestrictUpdate]
    [BsonId]
    public Guid Id { get; set; }
    
    [BsonRepresentation(BsonType.DateTime)]
    [BsonSerializer(typeof(DateTimeSerializer))]
    public DateTime CheckIn { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    [BsonSerializer(typeof(DateTimeSerializer))]
    public DateTime CheckOut { get; set; }
    
    [RestrictUpdate]
    public Guid UserId { get; set; }
    
    [RestrictUpdate]
    public Guid ListingId { get; set; }
    
    [BsonIgnoreIfNull]
    [RestrictUpdate]
    public Review? Review { get; set; }
    
    [BsonIgnoreIfNull]
    [BsonSerializer(typeof(Int32Serializer))]
    public int NumberOfGuests { get; set; }
    
    [BsonIgnoreIfNull]
    [BsonSerializer(typeof(Int32Serializer))]
    public int TotalPrice { get; set; }
    
    [BsonIgnoreIfNull]
    [BsonSerializer(typeof(BooleanSerializer))]
    public bool IsCancelled { get; set; }
    
}
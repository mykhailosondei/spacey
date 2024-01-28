using System.ComponentModel.DataAnnotations.Schema;
using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace ApplicationDAL.Entities;

public class Review
{
    [BsonId]
    [BsonIgnoreIfNull]
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    public string Comment { get; set; }
    
    public double[] Ratings { get; set; }
    
    [RestrictUpdate]
    public DateTime CreatedAt { get; set; }
    
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public Guid BookingId { get; set; }
    
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public Guid UserId { get; set; }
    
}
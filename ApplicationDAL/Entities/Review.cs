using System.ComponentModel.DataAnnotations.Schema;
using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Review
{
    [BsonId]
    [BsonIgnoreIfNull]
    [RestrictUpdate]
    public Guid Id { get; set; }
    
    public string Comment { get; set; }
    
    public double[] Ratings { get; set; }
    
    [RestrictUpdate]
    public Guid BookingId { get; set; }
    
    [RestrictUpdate]
    public Guid UserId { get; set; }
}
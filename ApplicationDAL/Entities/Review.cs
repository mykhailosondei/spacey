using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Review
{
    [BsonId]
    [BsonIgnoreIfNull]
    public Guid Id { get; set; }
    
    public string Comment { get; set; }
    
    public double[] Ratings { get; set; }
    
    public Guid BookingId { get; set; }
    
    public Guid UserId { get; set; }
}
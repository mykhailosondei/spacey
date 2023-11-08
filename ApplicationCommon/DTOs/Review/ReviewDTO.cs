using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Review;

public class ReviewDTO
{
    [BsonId]
    [BsonIgnoreIfNull]
    public Guid Id { get; set; }
    
    public string Comment { get; set; }
    
    public double[] Ratings { get; set; }
    
    public Guid BookingId { get; set; }
    
    public Guid UserId { get; set; }
}
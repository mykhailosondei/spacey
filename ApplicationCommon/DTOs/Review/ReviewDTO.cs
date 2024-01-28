using ApplicationCommon.Structs;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Review;

public class ReviewDTO
{
    [BsonId]
    [BsonIgnoreIfNull]
    public Guid Id { get; set; }
    
    public string Comment { get; set; }
    
    public Ratings Ratings { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid BookingId { get; set; }
    
    public Guid UserId{ get; set; }

}
using ApplicationCommon.Structs;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Review;

public class ReviewCreateDTO
{
    [BsonIgnoreIfNull]
    public Guid? Id { get; set; }
    
    public string Comment { get; set; }
    
    public Ratings Ratings { get; set; }
    
    public Guid BookingId { get; set; }
    
    public Guid UserId { get; set; }
}
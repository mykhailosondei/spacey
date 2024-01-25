using ApplicationCommon.Structs;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Review;

public class ReviewUpdateDTO
{
    [BsonIgnoreIfNull]
    public Guid? Id { get; set; }
    
    public string Comment { get; set; }
    
    public Ratings Ratings { get; set; }
}
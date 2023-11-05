using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Host
{
    [BsonId]
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int NumberOfListings { get; set; }
    
    public int NumberOfReviews { get; set; }
    
    public double Rating { get; set; }
    
    public List<Guid> ListingsIds { get; set; }
    
    List<Guid> ReviewsIds { get; set; }
}
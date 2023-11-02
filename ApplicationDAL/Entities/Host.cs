using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Host
{
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int NumberOfListings { get; set; }
    
    public int NumberOfReviews { get; set; }
    
    public double Rating { get; set; }
    
    public List<Listing> Listings { get; set; }
    
    List<Review> Reviews { get; set; }
}
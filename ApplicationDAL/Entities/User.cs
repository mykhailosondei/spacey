using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class User
{
    [BsonId]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string Password { get; set; }
    
    public string Address { get; set; }
    
    public string Description { get; set; }
    
    public string ProfilePictureUrl { get; set; }
    
    public List<Booking> Bookings { get; set; }
    
    public List<Review> Reviews { get; set; }
}
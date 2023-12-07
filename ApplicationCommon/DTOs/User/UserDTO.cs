using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.Interfaces;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.User;

public class UserDTO : ILastAccessible
{
    [BsonId]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Email { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string PasswordHash { get; set; }
    
    public string Address { get; set; }
    
    public string Description { get; set; }
    
    public ImageDTO Avatar { get; set; }
    
    public HostDTO Host { get; set; }
    
    public List<Guid> BookingsIds { get; set; }
    
    public List<Guid> ReviewsIds { get; set; }
    
    public List<Guid> LikedListingsIds { get; set; }
    
    public DateTime LastAccess { get; set; }

}
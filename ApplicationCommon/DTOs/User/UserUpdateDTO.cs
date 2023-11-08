using ApplicationCommon.DTOs.Host;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.User;

public class UserUpdateDTO
{
    public string Name { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    
    public string Description { get; set; }
    
    public string ProfilePictureUrl { get; set; }
}
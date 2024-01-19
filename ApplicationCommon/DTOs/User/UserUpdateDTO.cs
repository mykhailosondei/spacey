using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.Structs;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.User;

public class UserUpdateDTO
{
    public string Name { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public Address Address { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public string Description { get; set; }
    
    public ImageDTO Avatar { get; set; }
}
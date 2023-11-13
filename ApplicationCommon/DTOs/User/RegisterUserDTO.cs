namespace ApplicationCommon.DTOs.User;

public class RegisterUserDTO
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Name { get; set; }
    
    public string PhoneNumber { get; set; }
    
    public string Address { get; set; }
    
    public string? Description { get; set; }
}
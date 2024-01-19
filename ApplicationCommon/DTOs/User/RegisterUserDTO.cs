namespace ApplicationCommon.DTOs.User;

public class RegisterUserDTO
{
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string Name { get; set; }
    
    public DateTime BirthDate { get; set; }
}
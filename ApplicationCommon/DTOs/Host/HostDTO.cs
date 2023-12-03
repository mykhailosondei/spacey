namespace ApplicationCommon.DTOs.Host;

public class HostDTO
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public Guid UserId { get; set; }
    
    public int NumberOfListings { get; set; }
    
    public int NumberOfReviews { get; set; }
    
    public double Rating { get; set; }
    
    public List<Guid> ListingsIds { get; set; }
    
    public DateTime LastAccess { get; set; }
}
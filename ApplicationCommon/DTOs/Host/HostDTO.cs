namespace ApplicationCommon.DTOs.Host;

public class HostDTO
{
    public Guid Id { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public int NumberOfListings { get; set; }
    
    public int NumberOfReviews { get; set; }
    
    public double Rating { get; set; }
    
    public List<Guid> ListingsIds { get; set; }
    
    List<Guid> ReviewsIds { get; set; }
}
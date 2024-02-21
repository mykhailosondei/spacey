using System.Diagnostics.CodeAnalysis;

namespace ApplicationCommon.DTOs.Image;

public class ImageDTO
{
    public Guid? Id { get; set; }
    
    public string Url { get; set; }
}
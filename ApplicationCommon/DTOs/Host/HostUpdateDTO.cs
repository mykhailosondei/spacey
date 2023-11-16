using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Host;

public class HostUpdateDTO
{
    [BsonIgnoreIfNull]
    public Guid? Id { get; set; }
    
    [BsonIgnoreIfNull]
    public Guid? UserId { get; set; }
    
    public double Rating { get; set; }
}
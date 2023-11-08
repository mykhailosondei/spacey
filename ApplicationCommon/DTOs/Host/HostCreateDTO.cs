using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Host;

public class HostCreateDTO
{
    [BsonIgnoreIfNull]
    public Guid? Id { get; set; }
    
    public Guid UserId { get; set; }
}
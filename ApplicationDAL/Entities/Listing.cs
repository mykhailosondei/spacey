using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using ApplicationDAL.Attributes;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Listing
{
    [BsonId]
    [RestrictUpdate]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Address Address { get; set; }

    [BsonRepresentation(BsonType.String)]
    public PropertyType PropertyType { get; set; }
    
    public int PricePerNight { get; set; }

    public int NumberOfRooms { get; set; }

    public int NumberOfBathrooms { get; set; }

    public int NumberOfGuests { get; set; }
    
    public List<Image> ImagesUrls { get; set; }

    [RestrictUpdate]
    public Host Host { get; set; }

    [RestrictUpdate]
    public List<Guid> BookingsIds { get; set; }

    public Amenities Amenities { get; set; }
    
    public double[] Ratings { get; set; }
    
    public List<Guid> LikedUsersIds { get; set; }
    
    
}

using ApplicationCommon.DTOs.Image;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Listing;

public class ListingUpdateDTO
{
    public Guid? Id { get; set; }
    
    public string Title { get; set; }

    public string Description { get; set; }

    public string Address { get; set; }

    [BsonRepresentation(BsonType.String)]
    public PropertyType PropertyType { get; set; }
    
    public int PricePerNight { get; set; }

    public int NumberOfRooms { get; set; }

    public int NumberOfBathrooms { get; set; }

    public int NumberOfGuests { get; set; }

    public Guid HostId { get; set; }
    
    public List<ImageDTO> ImagesUrls { get; set; }

    public string[] Amenities { get; set; }
}
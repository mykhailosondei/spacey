using System.Net.Mime;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Listing;

public class ListingCreateDTO
{
    public string Title { get; set; }

    public string Description { get; set; }

    public Address Address { get; set; }

    [BsonRepresentation(BsonType.String)]
    public PropertyType PropertyType { get; set; }
    
    public int PricePerNight { get; set; }

    public int NumberOfRooms { get; set; }

    public int NumberOfBathrooms { get; set; }

    public int NumberOfGuests { get; set; }

    public List<ImageDTO> ImagesUrls { get; set; }

    public List<Guid> BookingsIds { get; set; }

    public List<Guid> ReviewsIds { get; set; }

    public string[] Amenities { get; set; }
}
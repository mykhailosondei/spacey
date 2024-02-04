using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.Enums;
using ApplicationCommon.Interfaces;
using ApplicationCommon.Structs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationCommon.DTOs.Listing;

public class ListingDTO : ILastAccessible
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Address Address { get; set; }

    [BsonRepresentation(BsonType.String)] public PropertyType PropertyType { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int PricePerNight { get; set; }

    public int NumberOfRooms { get; set; }

    public List<Ratings> Ratings { get; set; }

    public int NumberOfBathrooms { get; set; }

    public int NumberOfGuests { get; set; }

    public List<ImageDTO> ImagesUrls { get; set; }

    public HostDTO Host { get; set; }

    public List<Guid> BookingsIds { get; set; }

    public string[] Amenities { get; set; }
    
    public List<Guid> LikedUsersIds { get; set; }
    
    public DateTime LastAccess { get; set; }

}
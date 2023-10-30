using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApplicationDAL.Entities;

public class Listing
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public Address Address { get; set; }

    public PropertyType PropertyType { get; set; }

    public int PricePerNight { get; set; }

    public int NumberOfRooms { get; set; }

    public int NumberOfBathrooms { get; set; }

    public int NumberOfGuests { get; set; }

    public List<Image> ImageUrl { get; set; }

    public Host Host { get; set; }

    public List<Booking> Bookings { get; set; }

    public List<Review> Reviews { get; set; }

    public Amenities Amenities { get; set; }

}

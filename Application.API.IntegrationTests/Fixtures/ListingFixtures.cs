using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;

namespace Application.API.IntegrationTests.Fixtures;

public static class ListingFixtures
{
    public static readonly ListingCreateDTO ListingCreateDTO = new()
    {
        Title = "Test",
        Description = "Test",
        Address = new Address()
        {
            City = "Test",
            Country = "Test",
            Street = "Test"
        },
        PricePerNight = 10,
        NumberOfGuests = 10,
        NumberOfBathrooms = 10,
        NumberOfRooms = 10,
        Amenities = new string[]{},
        ImagesUrls = new List<ImageDTO>(),
        PropertyType = PropertyType.Apartment
    };
    
    public static readonly ListingCreateDTO ListingCreateDTOInvalid = new()
    {
        HostId = Guid.Empty,
        Title = "Test",
        Description = "Test",
        Address = new Address()
        {
            City = "Test",
            Country = "Test",
            Street = "Test"
        },
        PricePerNight = 10,
        NumberOfGuests = 10,
        NumberOfBathrooms = 10,
        NumberOfRooms = 10,
        Amenities = new string[]{},
        ImagesUrls = new List<ImageDTO>(),
        PropertyType = PropertyType.Apartment
    };
    
    public static readonly ListingUpdateDTO ListingUpdateDTO = new()
    {
        Title = "Test new",
        Description = "Test new",
        Address = new Address()
        {
            City = "Test new",
            Country = "Test new",
            Street = "Test new"
        },
        PricePerNight = 20,
        NumberOfGuests = 20,
        NumberOfBathrooms = 20,
        NumberOfRooms = 20,
        Amenities = new []{"Wifi","Washer","Gym"},
        ImagesUrls = new List<ImageDTO>(),
        PropertyType = PropertyType.Apartment
    };
    
    
}
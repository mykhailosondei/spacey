using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using MongoDB.Driver.GeoJsonObjectModel;

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
    
    public static readonly ListingCreateDTO ListingCreateNewYork1 = new()
    {
        Title = "Test",
        Description = "Test",
        Address = new Address()
        {
            City = "New York",
            Country = "United States",
            Street = "123 Main Street"
        },
        PricePerNight = 10,
        NumberOfGuests = 10,
        NumberOfBathrooms = 10,
        NumberOfRooms = 10,
        Amenities = new string[] { },
        ImagesUrls = new List<ImageDTO>(),
        PropertyType = PropertyType.Apartment
    };

    public static readonly ListingCreateDTO ListingCreateDTONewYork2 = new()
    {
        Title = "Cozy Apartment",
        Description = "A cozy apartment in the heart of New York City",
        Address = new Address()
        {
            City = "New York",
            Country = "United States",
            Street = "456 Broadway"
        },
        PricePerNight = 80,
        NumberOfGuests = 4,
        NumberOfBathrooms = 1,
        NumberOfRooms = 2,
        Amenities = new string[] { "Wifi", "Kitchen", "TV" },
        ImagesUrls = new List<ImageDTO>(),
        PropertyType = PropertyType.Apartment
    };

    public static readonly ListingCreateDTO ListingCreateDTONewYork3 = new()
    {
        Title = "Luxurious Penthouse",
        Description = "Experience luxury in this stunning penthouse with a view",
        Address = new Address()
        {
            City = "New York",
            Country = "United States",
            Street = "789 Park Avenue"
        },
        PricePerNight = 300,
        NumberOfGuests = 2,
        NumberOfBathrooms = 2,
        NumberOfRooms = 1,
        Amenities = new string[] { "Pool", "Gym", "HotTub" },
        ImagesUrls = new List<ImageDTO>(),
        PropertyType = PropertyType.Studio
    };

    public static readonly ListingCreateDTO ListingCreateDTONewYork4 = new()
    {
        Title = "Charming Brownstone",
        Description = "A charming brownstone in a historic neighborhood",
        Address = new Address()
        {
            City = "New York",
            Country = "United States",
            Street = "101 Maple Street"
        },
        PricePerNight = 150,
        NumberOfGuests = 6,
        NumberOfBathrooms = 2,
        NumberOfRooms = 3,
        Amenities = new string[] { "FreeParking", "FamilyFriendly", "Heating" },
        ImagesUrls = new List<ImageDTO>(),
        PropertyType = PropertyType.House
    };
    
    public static readonly ListingCreateDTO ListingCreateDTOInvalid = new()
    {
        Title = "Test",
        Description = "Test",
        Address = new Address()
        {
            City = "Test",
            Country = "Test",
            Street = "Test"
        },
        PricePerNight = 0,
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
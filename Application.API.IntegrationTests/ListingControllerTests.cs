using System.Net;
using System.Net.Http.Json;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using Xunit.Abstractions;

namespace Application.API.IntegrationTests;

public class ListingControllerTests : IntegrationTest
{

    public ListingControllerTests(ITestOutputHelper output) : base(output)
    {
        
    }


    [Fact]
    public async void PostListing_ReturnsSuccessStatus_OnValidInput()
    {
        //Arrange
        var hostCreate = new HostCreateDTO()
        {
            UserId = Guid.NewGuid()
        };
        var listing = new ListingCreateDTO()
        {
            HostId = new Guid(),
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
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    //TODO
    [Fact]
    public async void PostListing_ReturnsBadRequestStatus_OnInvalidInput()
    {
        //Arrange
        var listing = new ListingCreateDTO()
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
        //Act
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        //Assert
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void PostListing_SetsRightHost_OnValidInput()
    {
        var hostCreate = new HostCreateDTO()
        {
            UserId = Guid.NewGuid()
        };
        //Arrange
        var listing = new ListingCreateDTO()
        {
            HostId = new Guid(),
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
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        var responseGet = await TestClient.GetFromJsonAsync<ListingDTO>($"http://localhost:5241/api/Listing/{response.Content.ReadAsStringAsync().Result.Trim('"')}");
        
        //Assert
        Assert.True(responseGet.Host.Id == listing.HostId);
        Assert.Contains(responseGet.Id, responseGet.Host.ListingsIds);
    }
    
    

    [Fact]
    public async void UpdateListing_UpdatesTheListing_OnValidInput()
    {
        //Arrange
        var hostCreate = new HostCreateDTO()
        {
            UserId = Guid.NewGuid()
        };
        var listingCreate = new ListingCreateDTO()
        {
            HostId = new Guid("3faf1876-877a-4f65-bf52-e9e33d282b5a"),
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
            Amenities = new string[] { },
            ImagesUrls = new List<ImageDTO>(),
            PropertyType = PropertyType.Apartment

        };
        var listingUpdate = new ListingUpdateDTO()
        {
            Title = "Test2",
            Description = "Test2",
            Address = new Address()
            {
                City = "Test2",
                Country = "Test2",
                Street = "Test2"
            },
            PricePerNight = 20,
            NumberOfGuests = 20,
            NumberOfBathrooms = 20,
            NumberOfRooms = 20,
            Amenities = new string[] { },
            ImagesUrls = new List<ImageDTO>(),
            PropertyType = PropertyType.House
        };
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listingCreate.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var responsePost = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listingCreate);
        var responsePut = await TestClient.PutAsJsonAsync(
            $"http://localhost:5241/api/Listing/{responsePost.Content.ReadAsStringAsync().Result.Trim('"')}",
            listingUpdate);
        _output.WriteLine($"http://localhost:5241/api/Listing/{responsePost.Content.ReadAsStringAsync().Result.Trim('"')}");
        var responseGet = await TestClient.GetFromJsonAsync<ListingDTO>($"http://localhost:5241/api/Listing/{responsePost.Content.ReadAsStringAsync().Result.Trim('"')}");
        //Assert
        Assert.True(responseGet.Title == listingUpdate.Title);
        Assert.True(responseGet.Description == listingUpdate.Description);
        Assert.True(responseGet.Address.City == listingUpdate.Address.City);
        Assert.True(responseGet.Address.Country == listingUpdate.Address.Country);
        Assert.True(responseGet.Address.Street == listingUpdate.Address.Street);
        Assert.True(responseGet.PricePerNight == listingUpdate.PricePerNight);
        Assert.True(responseGet.NumberOfGuests == listingUpdate.NumberOfGuests);
        Assert.True(responseGet.NumberOfBathrooms == listingUpdate.NumberOfBathrooms);
    }

    [Fact]
    public async void DeleteListing_DeletesListingAndListingIdFromHost()
    {
        var hostCreate = new HostCreateDTO()
        {
            UserId = Guid.NewGuid()
        };
        //Arrange
        var listing = new ListingCreateDTO()
        {
            HostId = new Guid(),
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
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        await TestClient.DeleteAsync($"http://localhost:5241/api/Listing/{response.Content.ReadAsStringAsync().Result.Trim('"')}");
        //Assert
        var responseGet = await TestClient.GetStringAsync($"http://localhost:5241/api/Listing/{StringIdFromResponse(response)}");
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        var hostGet = await TestClient.GetFromJsonAsync<HostDTO>($"http://localhost:5241/api/Host/{StringIdFromResponse(hostCreateResponse)}");
        
        Assert.True(responseGet == "");
        Assert.DoesNotContain(new Guid(StringIdFromResponse(response)), hostGet.ListingsIds);
    }

    private static string StringIdFromResponse(HttpResponseMessage response)
    {
        return response.Content.ReadAsStringAsync().Result.Trim('"');
    }
}
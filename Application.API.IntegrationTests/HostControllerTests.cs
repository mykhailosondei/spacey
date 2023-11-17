using System.Net;
using System.Net.Http.Json;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using Xunit.Abstractions;

namespace Application.API.IntegrationTests;

public class HostControllerTests : IntegrationTest
{
    public HostControllerTests(ITestOutputHelper output) : base(output)
    {
        
    }

    [Fact]
    public async void PostHost_ReturnsSuccessStatus_OnValidInput()
    {
        //Arrange
        var host = new HostCreateDTO()
        {
            UserId = await GetUserId()
        };
        //Act
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", host);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        //Assert
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }
    
    [Fact]
    public async void PostHost_SetsRightUserId_OnValidInput()
    {
        //Arrange
        var host = new HostCreateDTO()
        {
            UserId = await GetUserId()
        };
        //Act
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", host);
        var responseGet = await TestClient.GetFromJsonAsync<HostDTO>($"http://localhost:5241/api/Host/{response.Content.ReadAsStringAsync().Result.Trim('"')}");
        
        //Assert
        Assert.True(responseGet.UserId == host.UserId);
    }
    
    [Fact]
    public async void PostHost_ReturnsBadRequestStatus_OnInvalidInput()
    {
        //Arrange
        var host = new HostCreateDTO()
        {
            UserId = Guid.Empty
        };
        //Act
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", host);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        //Assert
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async void UpdateHost_UpdatesTheHost_OnValidInput()
    {
        //Arrange
        var hostCreate = new HostCreateDTO()
        {
            UserId = await GetUserId()
        };
        var hostUpdate = new HostUpdateDTO()
        {
            Rating = 3.5d
        };
        //Act
        var responsePost = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        var responsePut = await TestClient.PutAsJsonAsync($"http://localhost:5241/api/Host/{responsePost.Content.ReadAsStringAsync().Result.Trim('"')}", hostUpdate);
        var responseGet = await TestClient.GetFromJsonAsync<HostDTO>($"http://localhost:5241/api/Host/{responsePost.Content.ReadAsStringAsync().Result.Trim('"')}");
        //Assert
        Assert.True(responseGet.Rating == hostUpdate.Rating);
    }
    
    [Fact]
    public async void UpdateHost_UpdatesHostInsideTheListing_OnValidInput()
    {
        //Arrange
        var hostCreate = new HostCreateDTO()
        {
            UserId = await GetUserId()
        };
        var hostUpdate = new HostUpdateDTO()
        {
            Rating = 3.5d
        };
        var listingCreate = new ListingCreateDTO()
        {
            HostId = Guid.NewGuid(),
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
        var responsePost = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listingCreate.HostId = Guid.Parse(responsePost.Content.ReadAsStringAsync().Result.Trim('"'));
        var responsePostListing = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listingCreate);
        Assert.True(responsePostListing.StatusCode == HttpStatusCode.OK);
        var responsePut = await TestClient.PutAsJsonAsync($"http://localhost:5241/api/Host/{responsePost.Content.ReadAsStringAsync().Result.Trim('"')}", hostUpdate);
        var responseGet = await TestClient.GetFromJsonAsync<ListingDTO>($"http://localhost:5241/api/Listing/{responsePostListing.Content.ReadAsStringAsync().Result.Trim('"')}");
        var responseHostGet = await TestClient.GetFromJsonAsync<HostDTO>($"http://localhost:5241/api/Host/{listingCreate.HostId}");
        Assert.Contains(responseGet.Host.ListingsIds, guid => guid == Guid.Parse(responsePostListing.Content.ReadAsStringAsync().Result.Trim('"')));
        Assert.Contains(responseHostGet.ListingsIds, guid => guid == Guid.Parse(responsePostListing.Content.ReadAsStringAsync().Result.Trim('"')));
        Assert.True(responseHostGet.Rating == hostUpdate.Rating);
        _output.WriteLine(responseGet.Host.Rating.ToString());
        _output.WriteLine(responseHostGet.Rating.ToString());
        Assert.True(responseGet.Host.Rating == hostUpdate.Rating);
    }

    [Fact]
    public async void UpdateHost_UpdatesHostInsideUser_OnValidInput()
    {
        //Arrange
        
        //Act

        //Assert
        Assert.True(false); //TODO: write after implement auth
    }
}
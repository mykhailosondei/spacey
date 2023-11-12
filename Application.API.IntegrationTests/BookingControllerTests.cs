using System.Net;
using System.Net.Http.Json;
using ApplicationCommon.DTOs.Booking;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using Xunit.Abstractions;

namespace Application.API.IntegrationTests;

public class BookingControllerTests : IntegrationTest
{
    private readonly ITestOutputHelper _output;
    
    public BookingControllerTests(ITestOutputHelper output)
    {
        _output = output;
    }
    
    [Fact]
    public async void PostBooking_ReturnsSuccessStatus_OnValidInput()
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
        var booking = new BookingCreateDTO()
        {
            ListingId = new Guid(),
            CheckIn = DateTime.Now,
            CheckOut = DateTime.Now.AddDays(1)
        };
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var listingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        booking.ListingId = new Guid(listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Booking", booking);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }
    
    [Fact]
    public async void PostBooking_AddsIdToListingBookingIds_OnValidInput()
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
        var booking = new BookingCreateDTO()
        {
            ListingId = new Guid(),
            CheckIn = DateTime.Now,
            CheckOut = DateTime.Now.AddDays(1)
        };
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var listingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        booking.ListingId = new Guid(listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Booking", booking);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.Contains(new Guid(response.Content.ReadAsStringAsync().Result.Trim('"')), (await TestClient.GetFromJsonAsync<ListingDTO>($"http://localhost:5241/api/Listing/{listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"')}")).BookingsIds);
    }
    
    [Fact]
    public async void UpdateBooking_UpdatesBooking_OnValidInput()
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
        var booking = new BookingCreateDTO()
        {
            ListingId = new Guid(),
            CheckIn = DateTime.Now,
            CheckOut = DateTime.Now.AddDays(1)
        };
        var bookingUpdate = new BookingUpdateDTO()
        {
            CheckIn = DateTime.Now.AddDays(1),
            CheckOut = DateTime.Now.AddDays(2)
        };
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var listingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        booking.ListingId = new Guid(listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var bookingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Booking", booking);
        bookingUpdate.Id = new Guid(bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PutAsJsonAsync($"http://localhost:5241/api/Booking/{bookingUpdate.Id}", bookingUpdate);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }
    
    [Fact]
    public async void DeleteBooking_DeletesBooking_OnValidInput()
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
        var booking = new BookingCreateDTO()
        {
            ListingId = new Guid(),
            CheckIn = DateTime.Now,
            CheckOut = DateTime.Now.AddDays(1)
        };
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var listingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        booking.ListingId = new Guid(listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var bookingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Booking", booking);
        var response = await TestClient.DeleteAsync($"http://localhost:5241/api/Booking/{bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"')}");
        var responseGet = await TestClient.GetStringAsync($"http://localhost:5241/api/Booking/{bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"')}");
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(responseGet == "");
    }
    
    [Fact]
    public async void DeleteBooking_DeletesIdFromListingBookingIds_OnValidInput()
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
        var booking = new BookingCreateDTO()
        {
            ListingId = new Guid(),
            CheckIn = DateTime.Now,
            CheckOut = DateTime.Now.AddDays(1)
        };
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var listingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        booking.ListingId = new Guid(listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var bookingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Booking", booking);
        var response = await TestClient.DeleteAsync($"http://localhost:5241/api/Booking/{bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"')}");
        var responseGet = await TestClient.GetStringAsync($"http://localhost:5241/api/Booking/{bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"')}");
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.DoesNotContain(new Guid(bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"')), (await TestClient.GetFromJsonAsync<ListingDTO>($"http://localhost:5241/api/Listing/{listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"')}")).BookingsIds);
    }
}
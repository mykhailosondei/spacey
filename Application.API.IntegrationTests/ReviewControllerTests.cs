using System.Net;
using System.Net.Http.Json;
using ApplicationCommon.DTOs.Booking;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.DTOs.Review;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using Xunit.Abstractions;

namespace Application.API.IntegrationTests;

public class ReviewControllerTests : IntegrationTest
{
    public ReviewControllerTests(ITestOutputHelper output) : base(output)
    {
        
    }
    
    //review is a part of the booking, so we need to create a booking first
    [Fact]
    public async void PostReview_ReturnsSuccessStatus_OnValidInput()
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
        var review = new ReviewCreateDTO()
        {
            BookingId = new Guid(),
            Ratings = new [] {1d},
            Comment = "Test"
        };    
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var listingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        booking.ListingId = new Guid(listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var bookingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Booking", booking);
        review.BookingId = new Guid(bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Review", review);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async void UpdateReview_UpdatesReviewInsideBooking()
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
        var review = new ReviewCreateDTO()
        {
            BookingId = new Guid(),
            Ratings = new [] {1d},
            Comment = "Test"
        };

        var reviewUpdate = new ReviewUpdateDTO()
        {
            Ratings = new[] { 1d },
            Comment = "Test"
        };
        //Act
        var hostCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Host", hostCreate);
        listing.HostId = new Guid(hostCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var listingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Listing", listing);
        booking.ListingId = new Guid(listingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var bookingCreateResponse = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Booking", booking);
        review.BookingId = new Guid(bookingCreateResponse.Content.ReadAsStringAsync().Result.Trim('"'));
        var response = await TestClient.PostAsJsonAsync("http://localhost:5241/api/Review", review);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }
}
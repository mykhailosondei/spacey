using System.Net;
using System.Net.Http.Json;
using Application.API.IntegrationTests.Fixtures;
using ApplicationCommon.DTOs.Booking;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.DTOs.Review;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using MongoDB.Bson;
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
        var listing = ListingFixtures.ListingCreateDTO;
        var booking = BookingFixtures.BookingCreateDTO;
        var review = ReviewFixtures.ReviewCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        _output.WriteLine(listing.ToBsonDocument().ToString());
        var listingCreateResponse = await Post<Listing>("api/Listing", listing);
        var listingId = await GetIdFromResponse(listingCreateResponse);
        booking.ListingId = listingId;
        await SwitchRole(false);
        var bookingCreateResponse = await Post<Booking>("api/Booking", booking);
        review.BookingId = await GetIdFromResponse(bookingCreateResponse);
        var response = await Post<Review>("api/Review", review);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async void UpdateReview_UpdatesReviewInsideBooking()
    {
        //Arrange
        var listing = ListingFixtures.ListingCreateDTO;
        var booking = BookingFixtures.BookingCreateDTO;
        var review = ReviewFixtures.ReviewCreateDTO;
        var reviewUpdate = ReviewFixtures.ReviewUpdateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        _output.WriteLine(listing.ToBsonDocument().ToString());
        var listingCreateResponse = await Post<Listing>("api/Listing", listing);
        var listingId = await GetIdFromResponse(listingCreateResponse);
        booking.ListingId = listingId;
        await SwitchRole(false);
        var bookingCreateResponse = await Post<Booking>("api/Booking", booking);
        review.BookingId = await GetIdFromResponse(bookingCreateResponse);
        var reviewCreateResponse = await Post<Review>("api/Review", review);
        var reviewId = await GetIdFromResponse(reviewCreateResponse);
        var response = await Put<Review>("api/Review/"+reviewId, reviewUpdate);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }
}
using System.Net;
using System.Net.Http.Json;
using Application.API.IntegrationTests.Fixtures;
using ApplicationCommon.DTOs.Booking;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using MongoDB.Bson;
using Serilog;
using Xunit.Abstractions;

namespace Application.API.IntegrationTests;


public class BookingControllerTests : IntegrationTest
{
    
    public BookingControllerTests(ITestOutputHelper output) : base(output)
    {
        
    }
    
    [Fact]
    public async void PostBooking_ReturnsSuccessStatus_OnValidInput()
    {
        var listing = ListingFixtures.ListingCreateDTO;
        var booking = BookingFixtures.BookingCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        _output.WriteLine(listing.ToBsonDocument().ToString());
        var listingCreateResponse = await Post<Listing>("api/Listing", listing);

        booking.ListingId = await GetIdFromResponse(listingCreateResponse);
        await SwitchRole(false);
        var response = await Post<Booking>("api/Booking", booking);
        //Assert
        
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async void PostBooking_AddsIdToListingBookingIds_OnValidInput()
    {
        //Arrange
        var listing = ListingFixtures.ListingCreateDTO;
        var booking = BookingFixtures.BookingCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        _output.WriteLine(listing.ToBsonDocument().ToString());
        var listingCreateResponse = await Post<Listing>("api/Listing", listing);
        var listingCreateId = await GetIdFromResponse(listingCreateResponse);
        booking.ListingId = listingCreateId;
        await SwitchRole(false);
        var response = await Post<Booking>("api/Booking", booking);
        var listingEntity =
            await GetObjectFromResponse<ListingDTO>(await Get<ListingDTO>($"api/Listing/{listingCreateId}"));
        //Assert
        _output.WriteLine(response.ToString());
        Assert.Contains(await GetIdFromResponse(response), listingEntity.BookingsIds);
    }

    [Fact]
    public async void UpdateBooking_UpdatesBooking_OnValidInput()
    {
        //Arrange
        var listing = ListingFixtures.ListingCreateDTO;
        var booking = BookingFixtures.BookingCreateDTO;
        var bookingUpdate = BookingFixtures.BookingUpdateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        _output.WriteLine(listing.ToBsonDocument().ToString());
        var listingCreateResponse = await Post<Listing>("api/Listing", listing);
        booking.ListingId = await GetIdFromResponse(listingCreateResponse);
        await SwitchRole(false);
        var bookingCreateResponse = await Post<Booking>("api/Booking", booking);
        bookingUpdate.Id = await GetIdFromResponse(bookingCreateResponse);
        var response = await Put<Booking>($"api/Booking/{bookingUpdate.Id}", bookingUpdate);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }
    
    [Fact]
    public async void DeleteBooking_DeletesBooking_OnValidInput()
    {
        var listing = ListingFixtures.ListingCreateDTO;
        var booking = BookingFixtures.BookingCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        _output.WriteLine(listing.ToBsonDocument().ToString());
        var listingCreateResponse = await Post<Listing>("api/Listing", listing);
        booking.ListingId = await GetIdFromResponse(listingCreateResponse);
        await SwitchRole(false);
        var bookingCreateResponse = await Post<Booking>("api/Booking", booking);
        var bookingCreateId = await GetIdFromResponse(bookingCreateResponse);
        var response = await Delete($"api/Booking/{bookingCreateId}");
        var responseGet = await Get<string>($"api/Booking/{bookingCreateId}");
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(responseGet.StatusCode == HttpStatusCode.UnprocessableEntity);
    }
    
    [Fact]
    public async void DeleteBooking_DeletesIdFromListingBookingIds_OnValidInput()
    {
        var listing = ListingFixtures.ListingCreateDTO;
        var booking = BookingFixtures.BookingCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        _output.WriteLine(listing.ToBsonDocument().ToString());
        var listingCreateResponse = await Post<Listing>("api/Listing", listing);
        var listingCreateId = await GetIdFromResponse(listingCreateResponse);
        booking.ListingId = listingCreateId;
        await SwitchRole(false);
        var bookingCreateResponse = await Post<Booking>("api/Booking", booking);
        var bookingCreateId = await GetIdFromResponse(bookingCreateResponse);
        var response = await Delete($"api/Booking/{bookingCreateId}");
        var listingEntity = await GetObjectFromResponse<ListingDTO>(await Get<ListingDTO>($"api/Listing/{listingCreateId}"));
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.DoesNotContain(await GetIdFromResponse(bookingCreateResponse), listingEntity.BookingsIds);
    }
}
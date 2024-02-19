using System.Net;
using System.Net.Http.Json;
using Application.API.IntegrationTests.Fixtures;
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationCommon.GeospatialUtilities;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
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
        var listing = new ListingCreateDTO()
        {
            HostId = new Guid(),
            Title = "Test",
            Description = "Test",
            Address = "Test, Test, Test",
            PricePerNight = 10,
            NumberOfGuests = 10,
            NumberOfBathrooms = 10,
            NumberOfRooms = 10,
            Amenities = new string[]{},
            ImagesUrls = new List<ImageDTO>(),
            PropertyType = PropertyType.Apartment
        };
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async void GetListingBySearch_NotContainsListing_OnUnavailableDate__Around()
    {
        var listing = ListingFixtures.ListingCreateDTO;

        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        
        var booking = new BookingCreateDTO()
        {
            ListingId = await GetIdFromResponse(response),
            CheckIn = DateTime.Parse(DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")),
            CheckOut = DateTime.Parse(DateTime.Now.AddDays(3).ToString("yyyy-MM-dd")),
            NumberOfGuests = 1,
            UserId = await GetUserId()
        };

        await SwitchRole(false);
        
        var bookingResponse = await Post<Booking>("api/Booking", booking);
        
        var responseGet = await Get<ListingDTO[]>("api/Listing/search?place=any&guests=1&checkIn="+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+"&checkOut="+DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"));
        
        var searchResult = await GetObjectFromResponse<ListingDTO[]>(responseGet);
        
        Assert.DoesNotContain(await GetIdFromResponse(response), searchResult.Select(x => x.Id));
    }
    
    [Fact]
    public async void GetListingBySearch_NotContainsListing_OnUnavailableDate__Exact()
    {
        var listing = ListingFixtures.ListingCreateDTO;

        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        
        await SwitchRole(false);
        
        var booking = new BookingCreateDTO()
        {
            ListingId = await GetIdFromResponse(response),
            CheckIn = DateTime.Parse(DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")),
            CheckOut = DateTime.Parse(DateTime.Now.AddDays(3).ToString("yyyy-MM-dd")),
            NumberOfGuests = 1,
            UserId = await GetUserId()
        };

        
        var bookingResponse = await Post<Booking>("api/Booking", booking);
        
        var responseGet = await Get<ListingDTO[]>("api/Listing/search?place=any&guests=1&checkIn="+DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")+"&checkOut="+DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"));
        
        var searchResult = await GetObjectFromResponse<ListingDTO[]>(responseGet);
        
        Assert.DoesNotContain(await GetIdFromResponse(response), searchResult.Select(x => x.Id));
    }
    
    [Fact]
    public async void GetListingBySearch_ContainsListing_OnAvailableDate()
    {
        var listing = ListingFixtures.ListingCreateDTO;

        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        
        var responseGet = await Get<ListingDTO[]>("api/Listing/search?place=any&guests=1&checkIn="+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+"&checkOut="+DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"));
        
        var searchResult = await GetObjectFromResponse<ListingDTO[]>(responseGet);
        
        Assert.Contains(await GetIdFromResponse(response), searchResult.Select(x => x.Id));
    }

    [Fact]
    public async void GetListingBySearch_NotContainsListing_Misc()
    {
        var listing = ListingFixtures.ListingCreateDTO;

        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        
        await SwitchRole(false);
        
        var userId = await GetUserId();
        
        var booking = new BookingCreateDTO()
        {
            ListingId = await GetIdFromResponse(response),
            CheckIn = DateTime.Parse(DateTime.Now.AddDays(2).ToString("yyyy-MM-dd")),
            CheckOut = DateTime.Parse(DateTime.Now.AddDays(3).ToString("yyyy-MM-dd")),
            NumberOfGuests = 1,
            UserId = userId
        };
        
        var booking2 = new BookingCreateDTO()
        {
            ListingId = await GetIdFromResponse(response),
            CheckIn = DateTime.Parse(DateTime.Now.AddDays(7).ToString("yyyy-MM-dd")),
            CheckOut = DateTime.Parse(DateTime.Now.AddDays(8).ToString("yyyy-MM-dd")),
            NumberOfGuests = 1,
            UserId = userId
        };
        
        var bookingResponse = await Post<Booking>("api/Booking", booking);
        var bookingResponse2 = await Post<Booking>("api/Booking", booking2);
        
        var responseGetAvailable = await Get<ListingDTO[]>("api/Listing/search?place=any&guests=1&checkIn="+DateTime.Now.AddDays(5).ToString("yyyy-MM-dd")+"&checkOut="+DateTime.Now.AddDays(6).ToString("yyyy-MM-dd"));
        
        var searchResultAvailable = await GetObjectFromResponse<ListingDTO[]>(responseGetAvailable);
        
        var responseGetUnavailable = await Get<ListingDTO[]>("api/Listing/search?place=any&guests=1&checkIn="+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+"&checkOut="+DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"));
        
        var searchResultUnavailable = await GetObjectFromResponse<ListingDTO[]>(responseGetUnavailable);
        
        Assert.DoesNotContain(await GetIdFromResponse(response), searchResultUnavailable.Select(x => x.Id));
        Assert.Contains(await GetIdFromResponse(response), searchResultAvailable.Select(x => x.Id));
    }
    
    [Fact]
    public async void PostListing_ReturnsValidIdOfExistingEntity_OnValidInput()
    {
        //Arrange
        var listing = ListingFixtures.ListingCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        var responseGet = await Get<ListingDTO>("api/Listing/"+await GetIdFromResponse(response));
        var listingGet = await GetObjectFromResponse<ListingDTO>(responseGet);
        //Assert
        Assert.True(listingGet.Id == await GetIdFromResponse(response));
    }
    
    [Fact]
    public async void PostListing_ReturnsBadRequestStatus_OnInvalidInput()
    {
        //Arrange
        var listing = ListingFixtures.ListingCreateDTOInvalid;
        //Act
        await SwitchRole(true);
        var response = await Post<Listing>("api/Listing", listing);
        //Assert
        Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
    }
    
    [Fact]
    public async void PostListing_SetsRightHost_OnValidInput()
    {
        var listing = ListingFixtures.ListingCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var listingResponse = await Post<Listing>("api/Listing", listing);
        //Assert
        _output.WriteLine(listingResponse.Content.ReadAsStringAsync().Result);
        var responseGet = await Get<ListingDTO>("api/Listing/"+await GetIdFromResponse(listingResponse));
        var listingGet = await GetObjectFromResponse<ListingDTO>(responseGet);
        //Assert
        Assert.True(listingGet.Host.Id == listing.HostId);
        Assert.Contains(listingGet.Id, listingGet.Host.ListingsIds);
    }
    
    

    [Fact]
    public async void UpdateListing_UpdatesTheListing_OnValidInput()
    {
        //Arrange
        var listingCreate = ListingFixtures.ListingCreateDTO;
        var listingUpdate = ListingFixtures.ListingUpdateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listingCreate.HostId = host.Id;
        var responsePost = await Post<ListingDTO>("api/Listing", listingCreate);
        var listingId = await GetIdFromResponse(responsePost);
        var responsePut = await Put<ListingDTO>("api/Listing/"+listingId, listingUpdate);
        var responseGet = await Get<ListingDTO>("api/Listing/"+listingId);
        var listing = await GetObjectFromResponse<ListingDTO>(responseGet);
        //Assert
        Assert.True(listing.Title == listingUpdate.Title);
        Assert.True(listing.Description == listingUpdate.Description);
        Assert.True(listing.PricePerNight == listingUpdate.PricePerNight);
        Assert.True(listing.NumberOfGuests == listingUpdate.NumberOfGuests);
        Assert.True(listing.NumberOfBathrooms == listingUpdate.NumberOfBathrooms);
    }

    [Fact]
    public async void DeleteListing_DeletesListingAndListingIdFromHost()
    {
        var listing = ListingFixtures.ListingCreateDTO;
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        listing.HostId = host.Id;
        var response = await Post<Listing>("api/Listing", listing);
        var listingId = await GetIdFromResponse(response);
        await Delete("api/Listing/" + listingId);
        //Assert
        var responseGet = await Get<Listing>("api/Listing/" + listingId);
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        var hostGet = await GetObjectFromResponse<Host>(await Get<Host>("api/Host/fromToken"));
        
        Assert.True(responseGet.StatusCode == HttpStatusCode.UnprocessableEntity);
        Assert.DoesNotContain(listingId, hostGet.ListingsIds);
    }
    
    [Fact]
    public async void GetListingsByBoundingBox_ReturnsAllListingsInsideBoundingBox()
    {
        //Arrange
        var listingsCreate = new List<ListingCreateDTO>()
        {
            ListingFixtures.ListingCreateNewYork1,
            ListingFixtures.ListingCreateDTONewYork2,
            ListingFixtures.ListingCreateDTONewYork2,
            ListingFixtures.ListingCreateDTONewYork3
        };
        //Act
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(hostResponse);
        List<Guid> listingIds = new();
        foreach (var listing in listingsCreate)
        {
            listing.HostId = host.Id;
            var response = await Post<Listing>("api/Listing", listing);
            listingIds.Add(await GetIdFromResponse(response));
        }
        var responseGet = await Get<ListingDTO[]>("api/Listing/boundingBox?x1=-75&y1=40&x2=-72&y2=42");
        var listings = await GetObjectFromResponse<ListingDTO[]>(responseGet);
        //Assert
        Assert.Contains(listingIds[0], listings.Select(x => x.Id));
        Assert.Contains(listingIds[1], listings.Select(x => x.Id));
        Assert.Contains(listingIds[2], listings.Select(x => x.Id));
        Assert.Contains(listingIds[3], listings.Select(x => x.Id));
    }
}
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
        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        //Assert
        _output.WriteLine(response.Content.ReadAsStringAsync().Result);
        Assert.True(response.StatusCode == HttpStatusCode.OK);
    }

    [Fact]
    public async void GetListingBySearch_ReturnsEmptyList_OnUnavailableDates()
    {
        var listing = ListingFixtures.ListingCreateDTO;

        await SwitchRole(true);
        var hostResponse = await Get<Host>("api/Host/fromToken");
        
        listing.HostId = (await GetObjectFromResponse<Host>(hostResponse)).Id;
        var response = await Post<Listing>("api/Listing", listing);
        
        var booking = new BookingCreateDTO()
        {
            ListingId = await GetIdFromResponse(response),
            CheckIn = DateTime.Now.AddDays(2),
            CheckOut = DateTime.Now.AddDays(3),
            NumberOfGuests = 1,
            UserId = await GetUserId()
        };

        await SwitchRole(false);
        
        var bookingResponse = await Post<Booking>("api/Booking", booking);
        
        _output.WriteLine("BBBBBBBBBBBBBBBBBBBBB"+bookingResponse.Content.ReadAsStringAsync().Result);
        
        var responseGet = await Get<ListingDTO[]>("api/Listing/search?place=any&guests=1&checkIn="+DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")+"&checkOut="+DateTime.Now.AddDays(5).ToString("yyyy-MM-dd"));
        
        Assert.True(responseGet.Content.ReadAsStringAsync().Result == "[]");
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
        Assert.True(listing.Address.City == listingUpdate.Address.City);
        Assert.True(listing.Address.Country == listingUpdate.Address.Country);
        Assert.True(listing.Address.Street == listingUpdate.Address.Street);
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
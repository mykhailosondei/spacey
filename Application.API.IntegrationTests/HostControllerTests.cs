using System.Net;
using System.Net.Http.Json;
using Application.API.IntegrationTests.Fixtures;
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
    public async void UpdateHost_UpdatesTheHost_OnValidInput()
    {
        //Arrange
        var hostUpdate = new HostUpdateDTO()
        {
            Rating = 3.5d
        };
        
        //Act
        await SwitchRole(true);
        var responsePost = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(responsePost);
        var responsePut = await Put<Host>("api/Host/"+host.Id, hostUpdate);
        var responseGet = await Get<Host>("api/Host/"+host.Id);
        host = await GetObjectFromResponse<Host>(responseGet);
        //Assert
        Assert.True(host.Rating == hostUpdate.Rating);
    }
    
    [Fact]
    public async void UpdateHost_UpdatesHostInsideTheListing_OnValidInput()
    {
        //Arrange
        var hostUpdate = new HostUpdateDTO()
        {
            Rating = Random.Shared.NextDouble()
        };
        var listingCreate = ListingFixtures.ListingCreateDTO;
        //Act
        await SwitchRole(true);
        var responsePost = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(responsePost);
        listingCreate.HostId = host.Id;
        var responsePostListing = await Post<ListingDTO>("api/Listing", listingCreate);
        var listingId = await GetIdFromResponse(responsePostListing);
        Assert.True(responsePostListing.StatusCode == HttpStatusCode.OK);
        var responsePut = await Put<Host>("api/Host/"+host.Id, hostUpdate);
        var responseGet = await Get<ListingDTO>("api/Listing/"+listingId);
        var listing = await GetObjectFromResponse<ListingDTO>(responseGet);
        var responseHostGet = await Get<Host>("api/Host/"+host.Id);
        host = await GetObjectFromResponse<Host>(responseHostGet);
        Assert.Contains(host.ListingsIds, guid => guid == listingId);
        Assert.Contains(listing.Host.ListingsIds, guid => guid == listingId);
        Assert.True(host.Rating == hostUpdate.Rating);
        Assert.True(listing.Host.Rating == hostUpdate.Rating);
    }

    [Fact]
    public async void UpdateHost_UpdatesHostInsideUser_OnValidInput()
    {
        //Arrange
        var hostUpdate = new HostUpdateDTO()
        {
            Rating = Random.Shared.NextDouble()
        };
        
        //Act
        await SwitchRole(true);
        var responsePost = await Get<Host>("api/Host/fromToken");
        var host = await GetObjectFromResponse<Host>(responsePost);
        var responsePut = await Put<Host>("api/Host/"+host.Id, hostUpdate);
        var responseGet = await Get<Host>("api/Host/"+host.Id);
        host = await GetObjectFromResponse<Host>(responseGet);
        await SwitchRole(false);
        var responseGetUser = await Get<User>("api/User/fromToken");
        var user = await GetObjectFromResponse<User>(responseGetUser);
        //Assert
        Assert.True(user.Host.Rating == hostUpdate.Rating);
    }
}
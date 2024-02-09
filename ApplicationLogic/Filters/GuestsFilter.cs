using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters;

public class GuestsFilter : AbstractFilter
{
    private readonly int _guests;

    public GuestsFilter(int guests)
    {
        _guests = guests;
    }
    
    public override async Task<List<ListingAndBookings>> ApplyFilter(List<ListingAndBookings> listings)
    {
        return listings.Where(listing => listing.Listing.NumberOfGuests >= _guests).ToList();
    }
}
using System.Globalization;
using System.Text;
using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Entities;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace ApplicationLogic.Filters;

public sealed class DateFilter : AbstractFilter
{
    private readonly DateTime? _checkIn;
    private readonly DateTime? _checkOut;

    public DateFilter(DateTime? checkIn, DateTime? checkOut)
    {
        _checkIn = checkIn;
        _checkOut = checkOut;
    }
    
    protected override bool IsEmpty()
    {
        return _checkIn == null && _checkOut == null;
    }
    
    public override async Task<List<ListingAndBookings>> ApplyFilter(List<ListingAndBookings> listings)
    {
        if (IsEmpty())
        {
            return listings;
        }

        (DateTime start, DateTime end)? range = null;
        
        if (_checkIn == null)
        {
            range ??= (_checkOut.Value, _checkOut.Value);
        }
        
        if (_checkOut == null)
        {
            range ??= (_checkIn.Value, _checkIn.Value);
        }
        
        range ??= (_checkIn.Value, _checkOut.Value);
        
        listings.ForEach(listing => 
            Console.WriteLine(listing.Bookings.Count())
            );
        return listings.Where(listing =>
            listing.Bookings.All(booking => booking.CheckOut < range.Value.start || booking.CheckIn > range.Value.end)).ToList();
    }
}
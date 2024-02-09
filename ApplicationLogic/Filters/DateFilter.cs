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

public class DateFilter : AbstractFilter
{
    private readonly DateTime _checkIn;
    private readonly DateTime _checkOut;

    public DateFilter(DateTime checkIn, DateTime checkOut)
    {
        _checkIn = checkIn;
        _checkOut = checkOut;
    }
    
    public override async Task<List<ListingAndBookings>> ApplyFilter(List<ListingAndBookings> listings)
    {
        return listings.Where(listing =>
            listing.Bookings.All(booking => booking.CheckOut < _checkIn || booking.CheckIn > _checkOut)).ToList();
    }
}
using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Entities;
using ApplicationLogic.Querying.QueryHandlers.ListingHandlers;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationLogic.Filters.Abstract;

public abstract class AbstractFilter
{
    public abstract Task<List<ListingAndBookings>> ApplyFilter(List<ListingAndBookings> listings);
    
    protected abstract bool IsEmpty();
}
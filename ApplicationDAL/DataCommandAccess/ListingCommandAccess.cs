using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class ListingCommandAccess : BaseAccessHandler, IListingDeletor
{
    private readonly IMongoCollection<Listing> _collection = GetCollection<Listing>("listings");
    private readonly IBookingDeletor _bookingDeletor;

    public ListingCommandAccess(IBookingDeletor bookingDeletor)
    {
        _bookingDeletor = bookingDeletor;
    }

    public async Task<Guid> AddListing(Listing listing)
    {
        listing.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(listing);
        await UpdateHostListingIdsOnListingAdd(listing.Host.Id, listing);
        return listing.Id;
    }
    
    private async Task UpdateHostListingIdsOnListingAdd(Guid id, Listing listing)
    {
        var hostFilter = Builders<Host>.Filter.Eq("Id", id);
        var hostUpdate = Builders<Host>.Update.Push("ListingsIds", listing.Id);
        await GetCollection<Host>("hosts").UpdateOneAsync(hostFilter, hostUpdate);
    }
    
    public async Task UpdateListing(Guid id, Listing listing)
    {
        var filter = Builders<Listing>.Filter.Eq("Id", id);
        listing.Id = id;
        await _collection.ReplaceOneAsync(filter, listing);
    }
    
    public async Task DeleteListing(Guid id)
    {
        var filter = Builders<Listing>.Filter.Eq("Id", id);
        await DeleteBookingsOnListingDelete(filter);
        await UpdateHostListingsIdsOnListingDelete(id);
        await _collection.DeleteOneAsync(filter);
    }
    
    private async Task DeleteBookingsOnListingDelete(FilterDefinition<Listing> filter)
    {
        List<Guid> bookingsIds = (await GetCollection<Listing>("listings").Find(filter).FirstOrDefaultAsync()).BookingsIds;
        foreach (var bookingId in bookingsIds)
        {
            await _bookingDeletor.DeleteBooking(bookingId);
        }
    }

    private async Task UpdateHostListingsIdsOnListingDelete(Guid id)
    {
        var hostFilter = Builders<Host>.Filter.In("ListingsIds", new[]{id});
        var hostUpdate = Builders<Host>.Update.Pull("ListingsIds", id);
        await GetCollection<Host>("hosts").UpdateOneAsync(hostFilter, hostUpdate);
    }
}
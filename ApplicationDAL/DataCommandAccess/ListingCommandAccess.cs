
using ApplicationDAL.Utilities;
using ApplicationDAL.Attributes;
using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class ListingCommandAccess : BaseAccessHandler, IListingDeletor, IListingCommandAccess
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
        Console.WriteLine(listing.Host.ToBsonDocument());
        await RetrieveHostOnListingAdd(listing.Host.Id, listing);
        if (listing.Host.ListingsIds == null)
        {
            listing.Host.ListingsIds = new List<Guid>();
        }
        listing.Host.ListingsIds.Add(listing.Id);
        await _collection.InsertOneAsync(listing);
        await UpdateHostListingIdsOnListingAdd(listing.Host.Id, listing);
        return listing.Id;
    }
    
    private async Task RetrieveHostOnListingAdd(Guid id, Listing listing)
    {
        var hostFilter = Builders<Host>.Filter.Eq("Id", id);
        listing.Host = await GetCollection<Host>("hosts").Find(hostFilter).FirstOrDefaultAsync();
    }
    
    private async Task UpdateHostListingIdsOnListingAdd(Guid id, Listing listing)
    {
        var hostFilter = Builders<Host>.Filter.Eq("Id", id);
        var hostUpdate = Builders<Host>.Update.Push("ListingsIds", listing.Id);
        Console.WriteLine
        ((await GetCollection<Host>("hosts").UpdateOneAsync(hostFilter, hostUpdate)).ModifiedCount);
        
    }
    
    public async Task UpdateListing(Guid id, Listing listing)
    {
        var filter = Builders<Listing>.Filter.Eq("Id", id);
        listing.Id = id;
        var documentToSet = listing.ToBsonDocument();
        //TODO: for all entities
        foreach (var name in ReflectionUtilities.GetPropertyNamesThatAreMarkedWithAttribute<RestrictUpdateAttribute>(typeof(Listing)))
        {
            Console.WriteLine(name);
            documentToSet.Remove(name);
        }
        var update = new BsonDocument("$set", documentToSet);
        await _collection.UpdateOneAsync(filter, update);
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
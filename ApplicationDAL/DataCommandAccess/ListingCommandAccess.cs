using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class ListingCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<Listing> _collection = GetCollection<Listing>("listings");
    
    public async Task<Guid> AddListing(Listing listing)
    {
        listing.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(listing);
        return listing.Id;
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
        await _collection.DeleteOneAsync(filter);
    }
}
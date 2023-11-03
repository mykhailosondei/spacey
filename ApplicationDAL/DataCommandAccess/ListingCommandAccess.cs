using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class ListingCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<Listing> _collection = GetCollection<Listing>("listings");
    
    public async Task AddListing(Listing listing)
    {
        await _collection.InsertOneAsync(listing);
    }
    
    public async Task UpdateListing(Guid id, Listing listing)
    {
        var filter = Builders<Listing>.Filter.Eq("Id", id);
        await _collection.ReplaceOneAsync(filter, listing);
    }
    
    public async Task DeleteListing(Guid id)
    {
        var filter = Builders<Listing>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}
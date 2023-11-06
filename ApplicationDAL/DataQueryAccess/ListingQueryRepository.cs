using ApplicationDAL.DataQueryAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataQueryAccess;

public class ListingQueryRepository : BaseQueryRepository
{
    private readonly IMongoCollection<Listing> _collection = GetCollection<Listing>("listings");
    
    public async Task<Listing> GetListingById(Guid id)
    {
        var filter = Builders<Listing>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<Listing>> GetAllListings()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }
    
    public async Task<IEnumerable<Listing>> GetListingsByHostId(Guid hostId)
    {
        var filter = Builders<Listing>.Filter.Eq("Host.Id", hostId);
        return await _collection.Find(filter).ToListAsync();
    }
}
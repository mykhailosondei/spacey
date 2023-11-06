using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class HostCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<Host> _collection = GetCollection<Host>("hosts");
    
    public async Task<Guid> AddHost(Host host)
    {
        host.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(host);
        return host.Id;
    }
    
    public async Task UpdateHost(Guid id, Host host)
    {
        var filter = Builders<Host>.Filter.Eq("Id", id);
        host.Id = id;
        await _collection.ReplaceOneAsync(filter, host);
        foreach (var listingId in host.ListingsIds)
        {
            Console.WriteLine(host.Rating);
            var filterListing = Builders<Listing>.Filter.Eq("Id", listingId);
            var update = Builders<Listing>.Update.Set("Host", host);
           await GetCollection<Listing>("listings").UpdateOneAsync(filterListing, update);
        }
    }
    
    public async Task DeleteHost(Guid id)
    {
        var filter = Builders<Host>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}
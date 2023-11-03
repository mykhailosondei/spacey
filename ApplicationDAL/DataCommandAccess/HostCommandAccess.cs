using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class HostCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<Host> _collection = GetCollection<Host>("hosts");
    
    public async Task AddHost(Host host)
    {
        await _collection.InsertOneAsync(host);
    }
    
    public async Task UpdateHost(Guid id, Host host)
    {
        var filter = Builders<Host>.Filter.Eq("Id", id);
        await _collection.ReplaceOneAsync(filter, host);
    }
    
    public async Task DeleteHost(Guid id)
    {
        var filter = Builders<Host>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}
using ApplicationCommon.Utilities;
using ApplicationDAL.Attributes;
using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class HostCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<Host> _collection = GetCollection<Host>("hosts");
    private readonly IListingDeletor _listingDeletor;

    public HostCommandAccess(IListingDeletor listingDeletor)
    {
        _listingDeletor = listingDeletor;
    }

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
        var update = new BsonDocument("$set", new BsonDocument(ReflectionUtilities.GetPropertiesThatAreNotMarkedWithAttribute<Host, RestrictUpdateAttribute>(host)));
        await _collection.UpdateOneAsync(filter, update);
        host = await GetCollection<Host>("hosts").Find(filter).FirstOrDefaultAsync();
        await UpdateHostsInAllListingsOnHostUpdate(host);
        await UpdateHostInUserOnHostModify(id, host);
    }

    private async Task UpdateHostsInAllListingsOnHostUpdate(Host host)
    {
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
        await DeleteListingsOnHostDelete(filter);
        await _collection.DeleteOneAsync(filter);
        await UpdateHostInUserOnHostModify(id, null);
    }
    
    private async Task UpdateHostInUserOnHostModify(Guid id, Host? host)
    {
        var userFilter = Builders<User>.Filter.Eq("Host.Id", id);
        var userUpdate = Builders<User>.Update.Set("Host", host);
        await GetCollection<User>("users").UpdateOneAsync(userFilter, userUpdate);
    }

    private async Task DeleteListingsOnHostDelete(FilterDefinition<Host> filter)
    {
        List<Guid> listingsIds = (await GetCollection<Host>("hosts").Find(filter).FirstOrDefaultAsync()).ListingsIds;
        foreach (var listingId in listingsIds)
        {
            await _listingDeletor.DeleteListing(listingId);
        }
    }
}
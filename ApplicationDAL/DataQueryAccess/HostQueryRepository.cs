using ApplicationDAL.DataQueryAccess.Abstract;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using MongoDB.Driver;

namespace ApplicationDAL.DataQueryAccess;

public class HostQueryRepository : BaseQueryRepository, IHostQueryRepository
{
    private readonly IMongoCollection<Host> _collection = GetCollection<Host>("hosts");
    
    public async Task<Host> GetHostById(Guid id)
    {
        var filter = Builders<Host>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
}
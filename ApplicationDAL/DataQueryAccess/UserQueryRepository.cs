using ApplicationDAL.DataQueryAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataQueryAccess;

public class UserQueryRepository : BaseQueryRepository
{
    private readonly IMongoCollection<User> _collection = GetCollection<User>("users");
    
    public async Task<User> GetUserById(Guid id)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        return await _collection.Find(filter).FirstOrDefaultAsync();
    }
    
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _collection.Find(new BsonDocument()).ToListAsync();
    }
}
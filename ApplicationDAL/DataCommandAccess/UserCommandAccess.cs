using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class UserCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<User> _collection = GetCollection<User>("users");

    public async Task AddUser(User user)
    {
        await _collection.InsertOneAsync(user);
    }

    public async Task UpdateUser(Guid id, User user)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        await _collection.ReplaceOneAsync(filter, user);
    }

    public async Task DeleteUser(Guid id)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}

    

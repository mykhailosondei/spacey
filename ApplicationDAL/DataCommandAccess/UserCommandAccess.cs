using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class UserCommandAccess : BaseAccessHandler
{
    private readonly IMongoCollection<User> _collection = GetCollection<User>("users");

    public async Task<Guid> AddUser(User user)
    {
        user.Id = Guid.NewGuid();
        await _collection.InsertOneAsync(user);
        return user.Id;
    }

    public async Task UpdateUser(Guid id, User user)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        user.Id = id;
        await _collection.ReplaceOneAsync(filter, user);
    }

    public async Task DeleteUser(Guid id)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}

    

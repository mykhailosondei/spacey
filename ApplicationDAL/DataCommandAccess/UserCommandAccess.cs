using ApplicationCommon.Utilities;
using ApplicationDAL.Attributes;
using ApplicationDAL.DataCommandAccess.Abstract;
using ApplicationDAL.Entities;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ApplicationDAL.DataCommandAccess;

public class UserCommandAccess : BaseAccessHandler, IUserCommandAccess
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
        var update = new BsonDocument("$set", new BsonDocument(ReflectionUtilities.GetPropertiesThatAreNotMarkedWithAttribute<User, RestrictUpdateAttribute>(user)));
        await _collection.UpdateOneAsync(filter, update);
    }

    public async Task DeleteUser(Guid id)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        await _collection.DeleteOneAsync(filter);
    }
}
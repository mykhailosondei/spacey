using MongoDB.Driver;

namespace ApplicationDAL.DbHelper;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string collectionName);
}
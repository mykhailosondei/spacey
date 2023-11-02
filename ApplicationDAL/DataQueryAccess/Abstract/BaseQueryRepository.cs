using MongoDB.Driver;

namespace ApplicationDAL.DataQueryAccess.Abstract;

public abstract class BaseQueryRepository
{
    private static readonly IMongoClient _client = new MongoClient("mongodb://localhost:27017");

    private static readonly IMongoDatabase _database = _client.GetDatabase("airbnb");
    
    protected static IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
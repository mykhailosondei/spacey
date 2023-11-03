using MongoDB.Driver;

namespace ApplicationDAL.DbHelper;

public abstract class CollectionGetter
{
    private static readonly IMongoClient _client = new MongoClient("mongodb://localhost:27017");

    private static readonly IMongoDatabase _database = _client.GetDatabase("airbnb");
    
    protected static IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
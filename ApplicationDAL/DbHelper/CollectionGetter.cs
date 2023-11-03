using MongoDB.Driver;

namespace ApplicationDAL.DbHelper;

public abstract class CollectionGetter
{
    private static readonly IMongoClient _client = new MongoClient("mongodb+srv://compassuser:wBzZ4kD5ejcI1FWf@democluster.4nn3xhe.mongodb.net/?retryWrites=true&w=majority");

    private static readonly IMongoDatabase _database = _client.GetDatabase("airbnb");
    
    protected static IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
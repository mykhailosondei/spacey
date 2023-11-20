using MongoDB.Driver;

namespace ApplicationDAL.DbHelper;

public abstract class CollectionGetter
{
    private static IMongoDbContext _mongoDbContext;

    public static void Initialize(IMongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext;
    }

    protected static IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _mongoDbContext.GetCollection<T>(collectionName);
    }
}
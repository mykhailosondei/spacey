using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace ApplicationDAL.DbHelper;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var settings = MongoClientSettings.FromConnectionString(connectionString);
        settings.LinqProvider = LinqProvider.V3;
        var client = new MongoClient(settings);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}
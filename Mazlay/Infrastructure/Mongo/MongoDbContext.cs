using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Mongo;

public class MongoDbContext
{
    public IMongoDatabase Db { get; }

    public MongoDbContext(IConfiguration cfg)
    {
        var client = new MongoClient(cfg.GetConnectionString("Mongo"));
        Db = client.GetDatabase(cfg["Mongo:Database"] ?? "MazlayDb");
    }
}
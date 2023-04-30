using AmazonMockup.Model.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AmazonMockup.DataAccess.Repositories;

public class BaseRepository<T> : IBaseRepository<T>
    where T : BaseMongoDbDocument
{
    private readonly IMongoCollection<T> _mongoDbCollection;

    public BaseRepository(IOptions<AmazonMockupDatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _mongoDbCollection = mongoDatabase.GetCollection<T>(databaseSettings.Value.CollectionName);
    }

    public async Task CreateOneAsync(T t, CancellationToken cancellationToken = default) =>
        await _mongoDbCollection.InsertOneAsync(t, cancellationToken: cancellationToken);

    public async Task CreateManyAsync(List<T> values, CancellationToken cancellationToken = default) =>
        await _mongoDbCollection.InsertManyAsync(values, cancellationToken: cancellationToken);

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var deleteResult = await _mongoDbCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);

        if (deleteResult.DeletedCount == 1)
        {
            return true;
        }

        return false;
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _mongoDbCollection.Find(_ => true).ToListAsync(cancellationToken);

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var filterDefinition = Builders<T>.Filter.Eq(x => x.Id, id);

        var documentCursor = await _mongoDbCollection.FindAsync<T>(filterDefinition, cancellationToken: cancellationToken);

        return await documentCursor.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> UpdateOneAsync(T t, CancellationToken cancellationToken = default)
    {
        var filterDefinition = Builders<T>.Filter.Eq(x => x.Id, t.Id);

        var resultCursor = await _mongoDbCollection.ReplaceOneAsync(filterDefinition, t, cancellationToken: cancellationToken);

        if (resultCursor.ModifiedCount == 1)
        {
            return true;
        }

        return false;
    }
}

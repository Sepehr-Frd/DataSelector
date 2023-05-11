namespace AmazonMockup.DataAccess;

public interface IBaseRepository<T> where T : class
{
    Task CreateOneAsync(T t, CancellationToken cancellationToken);

    Task CreateManyAsync(List<T> values, CancellationToken cancellationToken);

    Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);

    Task<bool> UpdateOneAsync(T t, CancellationToken cancellationToken = default);

    Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default);
}

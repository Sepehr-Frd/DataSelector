﻿using System.Net.Http.Headers;
using AmazonMockup.DataAccess;
using AmazonMockup.Model.Models;

namespace AmazonMockup.Business.Businesses;

public class BaseBusiness<T>
    where T : BaseMongoDbDocument
{
    private readonly IBaseRepository<T> _repository;

    public BaseBusiness(IBaseRepository<T> repository) =>
        _repository = repository;
    

    public async Task CreateOneAsync(T t, CancellationToken cancellationToken = default) =>
        await _repository.CreateOneAsync(t, cancellationToken);

    public async Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default) =>
        await _repository.GetByIdAsync(id, cancellationToken);

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await _repository.GetAllAsync(cancellationToken);

    public async Task<bool> UpdateOneAsync(T t, CancellationToken cancellationToken = default) =>
        await _repository.UpdateOneAsync(t, cancellationToken);

    public async Task<bool> DeleteByIdAsync(string id, CancellationToken cancellationToken = default) =>
        await _repository.DeleteByIdAsync(id, cancellationToken);
}


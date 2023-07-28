using DataSelector.Model.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nest;

namespace DataSelector.ExternalService.ElasticSearch;

public class ElasticSearchHostedService : IHostedService
{
    private readonly IElasticClient _elasticClient;

    private readonly string _indexName;

    private readonly IMongoCollection<QuestionDocument> _mongoDbCollection;

    public ElasticSearchHostedService(IElasticClient client, IOptions<MongoDbSettings> mongoDbSettings)
    {
        _elasticClient = client;
        _indexName = client.ConnectionSettings.DefaultIndex;

        var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
        _mongoDbCollection = mongoDatabase.GetCollection<QuestionDocument>(mongoDbSettings.Value.CollectionName);
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        await CreateDefaultIndexAsync(cancellationToken);

        await IndexAllQuestionsAsync(cancellationToken);
    }

    private async Task CreateDefaultIndexAsync(CancellationToken cancellationToken = default)
    {
        if ((await _elasticClient.Indices.ExistsAsync(_indexName, ct: cancellationToken)).Exists)
        {
            await _elasticClient.Indices.DeleteAsync(_indexName, ct: cancellationToken);
        }

        await _elasticClient.Indices.CreateAsync(_indexName, ct: cancellationToken, selector: c => c
                .Settings(s => s
                        .Analysis(a => a
                                .TokenFilters(tf => tf
                                        .Stop("english_stop", st => st
                                                .StopWords("_english_")
                                            )
                                        .Stemmer("english_stemmer", st => st
                                                .Language("english")
                                            )
                                        .Stemmer("light_english_stemmer", st => st
                                                .Language("light_english")
                                            )
                                        .Stemmer("english_possessive_stemmer", st => st
                                                .Language("possessive_english")
                                            )
                                    )
                                .Analyzers(aa => aa
                                        .Custom("light_english", ca => ca
                                                .Tokenizer("standard")
                                                .Filters("light_english_stemmer", "english_possessive_stemmer", "lowercase", "asciifolding")
                                            )
                                        .Custom("full_english", ca => ca
                                                .Tokenizer("standard")
                                                .Filters("english_possessive_stemmer",
                                                    "lowercase",
                                                    "english_stop",
                                                    "english_stemmer",
                                                    "asciifolding")
                                            )
                                        .Custom("full_english_synopsis", ca => ca
                                                .Tokenizer("standard")
                                                .Filters("english_possessive_stemmer",
                                                    "lowercase",
                                                    "english_stop",
                                                    "english_stemmer",
                                                    "asciifolding")
                                            )
                                    )
                            )
                    )
                .Map<QuestionDocument>(m => m
                        .AutoMap()
                        .Properties(p => p
                                .Text(t => t
                                        .Name(n => n.Title)
                                        .Analyzer("light_english")
                                    )
                                .Text(t => t
                                        .Name(n => n.Description)
                                        .Analyzer("full_english_synopsis")
                                    )
                            )
                    )
            );
    }

    private async Task<bool> IndexAllQuestionsAsync(CancellationToken cancellationToken = default)
    {
        var questions = await _mongoDbCollection.Find(_ => true).ToListAsync(cancellationToken);

        var indexManyResult = await _elasticClient.IndexManyAsync(questions, _indexName, cancellationToken);

        return !indexManyResult.Errors;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
using Bogus;
using DataSelector.Model.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataSelector.DataAccess.Repositories;

public class QuestionRepository : BaseRepository<QuestionDocument>
{
    private readonly IMongoCollection<QuestionDocument> _questionCollection;

    public QuestionRepository(IOptions<MongoDbSettings> databaseSettings) : base(databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);

        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);

        _questionCollection = mongoDatabase.GetCollection<QuestionDocument>(databaseSettings.Value.CollectionName);

        var count = _questionCollection.CountDocuments(FilterDefinition<QuestionDocument>.Empty);

        if (count == 0)
        {
            SeedQuestionsAsync();
        }
    }

    private void SeedQuestionsAsync()
    {
        var questionFaker = new Faker<QuestionDocument>()
            .RuleFor(question => question.Title, faker => faker.Lorem.Sentence(3))
            .RuleFor(question => question.Description, faker => faker.Lorem.Paragraph());

        var fakeQuestions = new List<QuestionDocument>();

        for (var i = 0; i < 10000; i++)
        {
            fakeQuestions.Add(questionFaker.Generate());
        }

        _questionCollection.InsertMany(fakeQuestions);
    }
}


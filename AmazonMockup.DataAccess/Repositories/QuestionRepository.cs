using AmazonMockup.Model.Models;
using Microsoft.Extensions.Options;

namespace AmazonMockup.DataAccess.Repositories;

public class QuestionRepository : BaseRepository<QuestionDocument>
{
    public QuestionRepository(IOptions<MongoDbSettings> databaseSettings) : base(databaseSettings)
    {
    }
}


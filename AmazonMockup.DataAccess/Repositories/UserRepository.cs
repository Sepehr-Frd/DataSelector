using AmazonMockup.Model.Models;
using Microsoft.Extensions.Options;

namespace AmazonMockup.DataAccess.Repositories;

public class UserRepository : BaseRepository<UserDocument>
{
    public UserRepository(IOptions<MongoDbSettings> databaseSettings) : base(databaseSettings)
    {
    }
}


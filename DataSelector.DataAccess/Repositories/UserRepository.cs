using DataSelector.Model.Models;
using Microsoft.Extensions.Options;

namespace DataSelector.DataAccess.Repositories;

public class UserRepository : BaseRepository<UserDocument>
{
    public UserRepository(IOptions<MongoDbSettings> databaseSettings) : base(databaseSettings)
    {
    }
}


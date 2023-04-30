using Amazon.Runtime.Internal.Util;
using AmazonMockup.Model.Models;
using Microsoft.Extensions.Options;

namespace AmazonMockup.DataAccess.Repositories;

public class UserRepository : BaseRepository<User>
{
    public UserRepository(IOptions<AmazonMockupDatabaseSettings> databaseSettings) : base(databaseSettings)
    {
    }
}


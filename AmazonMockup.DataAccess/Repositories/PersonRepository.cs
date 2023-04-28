using Amazon.Runtime.Internal.Util;
using AmazonMockup.Model.Models;
using Microsoft.Extensions.Options;

namespace AmazonMockup.DataAccess.Repositories;

public class PersonRepository : BaseRepository<Person>
{
    public PersonRepository(IOptions<AmazonMockupDatabaseSettings> databaseSettings) : base(databaseSettings)
    {
    }
}


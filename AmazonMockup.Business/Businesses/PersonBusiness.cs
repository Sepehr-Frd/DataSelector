using AmazonMockup.DataAccess;
using AmazonMockup.Model.Models;
using ZstdSharp.Unsafe;

namespace AmazonMockup.Business.Businesses;

public class PersonBusiness : BaseBusiness<Person>
{
    public PersonBusiness(IBaseRepository<Person> repository) : base(repository)
    {
    }

    public new async Task CreateOneAsync(Person person, CancellationToken cancellationToken = default)
    {
        var newPerson = new Person
        {
            FirstName = person.FirstName,
            LastName = person.LastName
        };

        await base.CreateOneAsync(newPerson, cancellationToken);
    }
}


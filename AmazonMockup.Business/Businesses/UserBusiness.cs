using AmazonMockup.DataAccess;
using AmazonMockup.Model.Models;
using ZstdSharp.Unsafe;

namespace AmazonMockup.Business.Businesses;

public class UserBusiness : BaseBusiness<User>
{
    public UserBusiness(IBaseRepository<User> repository) : base(repository)
    {
    }

    public new async Task CreateOneAsync(User user, CancellationToken cancellationToken = default)
    {
        var newUser = new User
        {
            Username = user.Username,
            Password = user.Password
        };

        await base.CreateOneAsync(newUser, cancellationToken);
    }
}


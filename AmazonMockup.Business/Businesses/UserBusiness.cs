using AmazonMockup.DataAccess;
using AmazonMockup.Model.Models;

namespace AmazonMockup.Business.Businesses;

public class UserBusiness : BaseBusiness<UserDocument>
{
    public UserBusiness(IBaseRepository<UserDocument> repository) : base(repository)
    {
    }

    public new async Task CreateOneAsync(UserDocument user, CancellationToken cancellationToken = default)
    {
        var newUser = new UserDocument
        {
            Username = user.Username,
            Password = user.Password
        };

        await base.CreateOneAsync(newUser, cancellationToken);
    }
}


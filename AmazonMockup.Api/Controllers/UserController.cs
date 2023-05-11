using AmazonMockup.Business.Businesses;
using AmazonMockup.Model.Models;

namespace AmazonMockup.Api.Controllers;

public class UserController : BaseController<UserDocument>
{
    public UserController(UserBusiness userBusiness) : base(userBusiness)
    {
    }
}


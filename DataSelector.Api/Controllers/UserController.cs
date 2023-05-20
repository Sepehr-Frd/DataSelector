using DataSelector.Business.Businesses;
using DataSelector.Model.Models;

namespace DataSelector.Api.Controllers;

public class UserController : BaseController<UserDocument>
{
    public UserController(UserBusiness userBusiness) : base(userBusiness)
    {
    }
}


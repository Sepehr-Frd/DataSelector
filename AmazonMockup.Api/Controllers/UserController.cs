using AmazonMockup.Business.Businesses;
using AmazonMockup.ExternalService;
using AmazonMockup.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmazonMockup.Api.Controllers;

public class UserController : BaseController<User>
{
    private readonly RedditMockupService _redditMockupService;

    private readonly UserBusiness _business;

    public UserController(BaseBusiness<User> userBusiness, RedditMockupService redditMockupService) : base(userBusiness)
    {
        _redditMockupService = redditMockupService;

        _business = (UserBusiness)userBusiness;
    }

    [HttpGet]
    [Route("/[action]")]
    public async Task<IActionResult> InsertPeopleInDatabase(CancellationToken cancellationToken)
    {
        var people = await _redditMockupService.GetPeopleAsync(cancellationToken);
        
        if (people is not null)
        {
            await _business.CreateManyAsync(people, cancellationToken);
        }

        return Ok();
    }

}


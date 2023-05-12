using AmazonMockup.Business.Businesses;
using AmazonMockup.ExternalService;
using AmazonMockup.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace AmazonMockup.Api.Controllers;

public class QuestionController : BaseController<QuestionDocument>
{
    private readonly QuestionBusiness _questionBusiness;

    private readonly RedditMockupService _redditMockupService;

    public QuestionController(QuestionBusiness questionBusiness, RedditMockupService redditMockupService) : base(questionBusiness)
    {
        _questionBusiness = questionBusiness;

        _redditMockupService = redditMockupService;
    }

    [HttpGet]
    public async Task<IActionResult> ImportQuestionsAsync(CancellationToken cancellationToken)
    {
        var questions = await _redditMockupService.GetQuestionsAsync(cancellationToken);

        if (questions is null || questions.Count == 0)
        {
            return NoContent();
        }

        await _questionBusiness.CreateManyAsync(questions, cancellationToken);

        return Ok();
    }

}


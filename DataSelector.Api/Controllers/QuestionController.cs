using DataSelector.Business.Businesses;
using DataSelector.Common.Dtos;
using DataSelector.ExternalService.ElasticSearch;
using DataSelector.ExternalService.RedditMockup;
using DataSelector.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataSelector.Api.Controllers;

[Route("questions")]
public class QuestionController : BaseController<QuestionDocument>
{
    private readonly QuestionBusiness _questionBusiness;

    private readonly RedditMockupRestService _redditMockupService;

    private readonly ElasticSearchService _elasticSearchService;

    public QuestionController(QuestionBusiness questionBusiness, RedditMockupRestService redditMockupService, ElasticSearchService elasticSearchService) : base(questionBusiness)
    {
        _questionBusiness = questionBusiness;

        _redditMockupService = redditMockupService;

        _elasticSearchService = elasticSearchService;
    }

    [HttpGet]
    [Route("import-from-external-resource")]
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

    [HttpGet]
    [Route("elasticsearch")]
    public async Task<SearchResponseDto<QuestionResponseDto>?> ElasticSearchQueryAsync([FromQuery] string query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return null;
        }

        return await _elasticSearchService.QueryQuestions(query, cancellationToken);
    }

}
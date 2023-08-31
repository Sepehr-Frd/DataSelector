using DataSelector.Business.Businesses;
using DataSelector.Common.Dtos;
using DataSelector.ExternalService.ElasticSearch;
using DataSelector.Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace DataSelector.Api.Controllers;

[Route("questions")]
public class QuestionController : BaseController<QuestionDocument>
{
    private readonly QuestionBusiness _questionBusiness;

    private readonly ElasticSearchService _elasticSearchService;

    public QuestionController(QuestionBusiness questionBusiness, ElasticSearchService elasticSearchService) : base(questionBusiness)
    {
        _questionBusiness = questionBusiness;
        _elasticSearchService = elasticSearchService;
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
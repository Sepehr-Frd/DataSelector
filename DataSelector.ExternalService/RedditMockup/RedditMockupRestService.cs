using AutoMapper;
using DataSelector.Common.Dtos;
using DataSelector.Model.Models;
using Newtonsoft.Json;
using RestSharp;

namespace DataSelector.ExternalService.RedditMockup;

public class RedditMockupRestService
{
    private const string BaseAddress = "http://localhost:6000/public/questions";

    private readonly IMapper _mapper;

    public RedditMockupRestService(IMapper mapper) =>
        _mapper = mapper;

    public async Task<List<QuestionDocument>?> GetQuestionsAsync(CancellationToken cancellationToken = default)
    {
        var restClient = new RestClient();

        var restRequest = new RestRequest($"{BaseAddress}/Question")
        {
            Timeout = TimeSpan.FromSeconds(5).Milliseconds
        };

        var restResponse = await restClient.ExecuteGetAsync(restRequest, cancellationToken);

        var deserializedResponse = await Task.Factory.StartNew(
            () => JsonConvert.DeserializeObject<RedditMockupResponseDto<List<QuestionResponseDto>>>(restResponse.Content ?? ""));

        var questions = _mapper.Map<List<QuestionDocument>>(deserializedResponse?.Data);

        return questions;
    }
}
using AutoMapper;
using DataSelector.Common.Dtos;
using DataSelector.Model.Models;
using Nest;

namespace DataSelector.ExternalService.ElasticSearch;

public class ElasticSearchService
{
    private readonly IElasticClient _elasticClient;
    private readonly IMapper _mapper;

    public ElasticSearchService(IElasticClient elasticClient, IMapper mapper)
    {
        _elasticClient = elasticClient;
        _mapper = mapper;
    }

    public async Task<SearchResponseDto<QuestionResponseDto>> QueryQuestions(string query, CancellationToken cancellationToken)
    {
        var response = await _elasticClient.SearchAsync<QuestionDocument>(s =>
                s.Query(sq =>
                        sq.MultiMatch(mm => mm
                                .Query(query)
                                .Fuzziness(Fuzziness.Auto)
                            )
                    ),
            cancellationToken);
        
        var searchDto = new SearchResponseDto<QuestionResponseDto>
        {
            Term = query
        };

        if (response.IsValid)
        {
            searchDto.Result = _mapper.Map<List<QuestionResponseDto>>(response.Documents?.ToList());
        }

        return searchDto;
    }

}
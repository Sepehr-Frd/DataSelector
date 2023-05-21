using AutoMapper;
using DataSelector.Common.Dtos;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using RedditMockup;

namespace DataSelector.ExternalService.RedditMockup.RedditMockupGrpcService;

public class RedditMockupDataClient : IRedditMockupDataClient
{
    private readonly IConfiguration _configuration;

    private readonly IMapper _mapper; 

    public RedditMockupDataClient(IConfiguration configuration, IMapper mapper)
    {
        _configuration = configuration;
        _mapper = mapper;
    }

    public IEnumerable<QuestionResponseDto> ReturnAllQuestions()
    {
        var channel = GrpcChannel.ForAddress(_configuration.GetValue<string>("RedditMockupGrpc")!);
        
        var client = new RedditMockupGrpc.RedditMockupGrpcClient(channel);

        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllQuestions(request);
            return _mapper.Map<IEnumerable<QuestionResponseDto>>(reply);
        }
        catch (Exception exception)
        {
            // TODO: Use Nlog
            Console.WriteLine($"Could not call GRPC Server {exception.Message}");
        }
    }
}

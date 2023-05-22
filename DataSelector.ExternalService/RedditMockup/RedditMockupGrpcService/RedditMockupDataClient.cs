using System.Net;
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

    public IEnumerable<QuestionResponseDto>? ReturnAllQuestions()
    {
        var grpcAddress = _configuration.GetValue<string>("RedditMockupGrpc");

        if (grpcAddress is null)
        {
            // TODO: Use NLog
            Console.WriteLine("RedditMockupGrpc address in appsettings is null or invalid!");

            return null;
        }


        var handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback =
            HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials,
            UseDefaultCredentials = true

        };

        var channel = GrpcChannel.ForAddress("http://localhost:6000",
            new GrpcChannelOptions { HttpHandler = handler });

        //var proxy = new WebProxy
        //{
        //    UseDefaultCredentials = true,
        //    BypassProxyOnLocal = true,
        //    Credentials = CredentialCache.DefaultNetworkCredentials
        //};

        //proxy.BypassArrayList.Add("https://localhost:6000");

        //var httpClientHandler = new HttpClientHandler
        //{
        //    Proxy = proxy,
        //    DefaultProxyCredentials = CredentialCache.DefaultNetworkCredentials,
        //    UseDefaultCredentials = true
        //};
        //var httpClient = new HttpClient
        //{
        //    DefaultRequestVersion = HttpVersion.Version20,
        //    DefaultVersionPolicy = HttpVersionPolicy.RequestVersionExact
        //};

        //var channel = GrpcChannel.ForAddress(grpcAddress, new GrpcChannelOptions
        //{
        //    HttpClient = httpClient
        //});

        //var channel = GrpcChannel.ForAddress(grpcAddress);


        var client = new RedditMockupGrpc.RedditMockupGrpcClient(channel);

        var request = new GetAllRequest();

        try
        {
            var reply = client.GetAllQuestions(request);
            return _mapper.Map<IEnumerable<QuestionResponseDto>>(reply.Question);
        }
        catch (Exception exception)
        {
            // TODO: Use NLog
            Console.WriteLine($"Could not call GRPC Server {exception.Message}");

            return null;
        }
    }
}

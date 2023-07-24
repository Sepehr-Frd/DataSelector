using DataSelector.Business.Businesses;
using DataSelector.Common.Profiles;
using DataSelector.DataAccess;
using DataSelector.DataAccess.Repositories;
using DataSelector.ExternalService.RabbitMQ;
using DataSelector.ExternalService.RabbitMQ.EventProcessing;
using DataSelector.ExternalService.RedditMockup;
using DataSelector.ExternalService.RedditMockup.RedditMockupGrpcService;
using DataSelector.Model.Models;
using Nest;

namespace DataSelector.Web;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection InjectControllers(this IServiceCollection services) => services.AddControllers().Services;

    public static IServiceCollection InjectDatabaseSettings(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));

    public static IServiceCollection InjectRepositories(this IServiceCollection services) =>
        services.AddScoped<IBaseRepository<QuestionDocument>, QuestionRepository>();

    public static IServiceCollection InjectBusinesses(this IServiceCollection services) =>
        services.AddScoped<QuestionBusiness>();

    public static IServiceCollection InjectServices(this IServiceCollection services) =>
        services.AddScoped<RedditMockupRestService>();

    internal static IServiceCollection InjectAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(QuestionProfile).Assembly);

    internal static IServiceCollection InjectExternalServices(this IServiceCollection services) =>
        services
            .AddSingleton<IEventProcessor, EventProcessor>()
            .AddScoped<IRedditMockupDataClient, RedditMockupDataClient>();

    internal static IServiceCollection InjectMessageBusSubscriber(this IServiceCollection services) =>
        services.AddHostedService<MessageBusSubscriber>();

    internal static IServiceCollection InjectElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("ElasticSearch").GetValue<string>("ConnectionString");
        var defaultIndex = configuration.GetSection("ElasticSearch").GetValue<string>("DefaultIndex");

        var settings = new ConnectionSettings(new Uri(connectionString!));

        settings = settings.DefaultIndex(defaultIndex);

        var elasticClient = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(elasticClient);

        return services;
    }
}


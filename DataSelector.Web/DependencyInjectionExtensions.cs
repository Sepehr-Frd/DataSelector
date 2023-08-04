using DataSelector.Business.Businesses;
using DataSelector.Common.Profiles;
using DataSelector.DataAccess;
using DataSelector.DataAccess.Repositories;
using DataSelector.ExternalService.ElasticSearch;
using DataSelector.ExternalService.RedditMockup;
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

    public static IServiceCollection InjectExternalServices(this IServiceCollection services) =>
        services.AddScoped<RedditMockupRestService>();

    internal static IServiceCollection InjectAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(QuestionProfile).Assembly);

    internal static IServiceCollection InjectElasticSearch(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetSection("ElasticSearch").GetValue<string>("ConnectionString");

        var defaultIndex = configuration.GetSection("ElasticSearch").GetValue<string>("DefaultIndex");

        var settings = new ConnectionSettings(new Uri(connectionString!))
            .CertificateFingerprint("D3:08:86:DB:2E:46:C4:0B:40:E9:29:17:C7:7A:50:A1:9C:17:B7:FE:7C:45:93:B9:09:9B:40:F1:C4:79:BF:24")
            .DefaultIndex(defaultIndex)
            .BasicAuthentication("elastic", "pKSNgA_5gAxzrW4CHu1D")
            .EnableApiVersioningHeader();

        var elasticClient = new ElasticClient(settings);

        services.AddSingleton<IElasticClient>(elasticClient);

        services.AddHostedService<ElasticSearchHostedService>();

        services.AddScoped<ElasticSearchService>();

        return services;
    }
}
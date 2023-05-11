using AmazonMockup.Business.Businesses;
using AmazonMockup.Common.MappingProfiles;
using AmazonMockup.DataAccess;
using AmazonMockup.DataAccess.Repositories;
using AmazonMockup.ExternalService;
using AmazonMockup.Model.Models;

namespace AmazonMockup.Web;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection InjectControllers(this IServiceCollection services) => services.AddControllers().Services;

    public static IServiceCollection InjectDatabaseSettings(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDb"));

    public static IServiceCollection InjectRepositories(this IServiceCollection services) =>
        services.AddScoped<IBaseRepository<UserDocument>, UserRepository>()
                .AddScoped<IBaseRepository<QuestionDocument>, QuestionRepository>();

    public static IServiceCollection InjectBusinesses(this IServiceCollection services) =>
        services.AddScoped<UserBusiness>()
                .AddScoped<QuestionBusiness>();

    public static IServiceCollection InjectServices(this IServiceCollection services) =>
        services.AddScoped<RedditMockupService>();

    internal static IServiceCollection InjectAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(QuestionProfile).Assembly);
}


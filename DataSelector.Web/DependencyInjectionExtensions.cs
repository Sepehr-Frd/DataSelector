using DataSelector.Business.Businesses;
using DataSelector.Common.MappingProfiles;
using DataSelector.DataAccess;
using DataSelector.DataAccess.Repositories;
using DataSelector.ExternalService;
using DataSelector.ExternalService.RabbitMQ;
using DataSelector.ExternalService.RabbitMQ.EventProcessing;
using DataSelector.Model.Models;

namespace DataSelector.Web;

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

    internal static IServiceCollection InjectEventProcessor(this IServiceCollection services) =>
        services.AddSingleton<IEventProcessor, EventProcessor>();

    internal static IServiceCollection InjectMessageBusSubscriber(this IServiceCollection services) =>
        services.AddHostedService<MessageBusSubscriber>();
}


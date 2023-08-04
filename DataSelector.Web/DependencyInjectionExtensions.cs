using DataSelector.Business.Businesses;
using DataSelector.Common.Profiles;
using DataSelector.DataAccess;
using DataSelector.DataAccess.Repositories;
using DataSelector.Model.Models;

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

    internal static IServiceCollection InjectAutoMapper(this IServiceCollection services) =>
        services.AddAutoMapper(typeof(QuestionProfile).Assembly);
}


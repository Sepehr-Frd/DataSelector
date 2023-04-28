using System;
using System.Runtime.CompilerServices;
using AmazonMockup.Business.Businesses;
using AmazonMockup.DataAccess;
using AmazonMockup.DataAccess.Repositories;
using AmazonMockup.Model.Models;

namespace AmazonMockup.Web;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection InjectControllers(this IServiceCollection services) => services.AddControllers().Services;
    

    public static IServiceCollection InjectDatabaseSettings(this IServiceCollection services, IConfiguration configuration) =>
        services.Configure<AmazonMockupDatabaseSettings>(configuration.GetSection("MongoDb"));

    public static IServiceCollection InjectRepositories(this IServiceCollection services) =>
        services.AddScoped<IBaseRepository<Person>, PersonRepository>();

    public static IServiceCollection InjectBusinesses(this IServiceCollection services) =>
        services.AddScoped<BaseBusiness<Person>, PersonBusiness>();

}


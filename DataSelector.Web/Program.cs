using DataSelector.Web;
var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .InjectDatabaseSettings(builder.Configuration)
    .InjectRepositories()
    .InjectBusinesses()
    .InjectControllers()
    .InjectAutoMapper();

var app = builder.Build();

app.UseSwagger()
    .UseSwaggerUI();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();

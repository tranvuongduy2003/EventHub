using EventHub.Infrastructure;
using EventHub.Infrastructure.Logging;
using EventHub.Persistence;
using EventHub.Presentation.Extensions;
using EventHub.SignalR;
using Serilog;

var AppCors = "AppCors";

var builder = WebApplication.CreateBuilder(args);

Log.Information("Starting EvenHub API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.Host.AddAppConfigurations();

    builder.Services.ConfigurePersistenceServices(builder.Configuration);
    builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);
    builder.Services.ConfigureSignalRServices(builder.Configuration);

    var app = builder.Build();

    app.UseInfrastructure(AppCors);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");

    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal)) throw;
}
finally
{
    Log.Information("Shut down EventHub API complete");
    Log.CloseAndFlush();
}
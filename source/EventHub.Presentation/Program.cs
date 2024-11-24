using EventHub.Infrastructure;
using EventHub.Infrastructure.Configurations;
using EventHub.Persistence;
using EventHub.Presentation.Extensions;
using EventHub.SignalR;
using Serilog;

string AppCors = "AppCors";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Information("Starting EvenHub API up");

builder.Host.UseSerilog(LoggingConfiguration.Configure);
builder.AddAppConfigurations();

builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);
builder.Services.ConfigureSignalRServices();

WebApplication app = builder.Build();

app.UseInfrastructure(AppCors);

app.Run();

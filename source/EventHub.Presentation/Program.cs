using EventHub.Domain.Shared.Settings;
using EventHub.Infrastructure;
using EventHub.Infrastructure.Configurations;
using EventHub.Presentation.Extensions;
using Serilog;

string AppCors = "AppCors";

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Log.Information("Starting EvenHub API up");

builder.Host.UseSerilog(LoggingConfiguration.Configure);
builder.AddAppConfigurations();

builder.Services.ConfigureInfrastructureServices(builder.Configuration, AppCors);

Stripe.StripeConfiguration.ApiKey = builder.Services.GetOptions<StripeSettings>("StripeSettings").SecretKey;

WebApplication app = builder.Build();

app.UseInfrastructure(AppCors);

app.Run();

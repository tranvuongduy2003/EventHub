using System.Security.Authentication;
using EventHub.Application.Abstractions;
using EventHub.Domain.Shared.Settings;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace EventHub.Infrastructure.Configurations;

public static class HangfireConfiguration
{
    public static IServiceCollection ConfigureHangfireServices(this IServiceCollection services)
    {
        HangfireSettings hangfireSettings = services.GetOptions<HangfireSettings>("HangfireSettings");
        if (hangfireSettings == null
            || hangfireSettings.Storage == null
            || string.IsNullOrEmpty(hangfireSettings.Storage.ConnectionString)
            || string.IsNullOrEmpty(hangfireSettings.Storage.DBProvider))
        {
            throw new NullReferenceException("HangfireSettings is not configured properly!");
        }

        var mongoUrlBuilder = new MongoUrlBuilder(hangfireSettings.Storage.ConnectionString);

        var mongoClientSettings = MongoClientSettings.FromUrl(
            new MongoUrl(hangfireSettings.Storage.ConnectionString));
        mongoClientSettings.SslSettings = new SslSettings
        {
            EnabledSslProtocols = SslProtocols.None
        };
        var mongoClient = new MongoClient(mongoClientSettings);
        var mongoStorageOptions = new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            CheckConnection = true,
            Prefix = "SchedulerQueue",
            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.TailNotificationsCollection
        };

        services.AddHangfire((provider, config) =>
        {
            config.UseSimpleAssemblyNameTypeSerializer()
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, mongoStorageOptions);

            var jsonSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            config.UseSerializerSettings(jsonSettings);
        });
        services.AddHangfireConsoleExtensions();
        services.AddHangfireServer(options => { options.ServerName = hangfireSettings.ServerName; });

        return services;
    }

    public static IApplicationBuilder UseHangfireBackgroundJobs(this IApplicationBuilder app)
    {
        app.ApplicationServices
            .GetRequiredService<IRecurringJobManager>()
            .AddOrUpdate<IProcessOutboxMessagesJob>(
                "outbox-processor",
                (job) => job.Execute(null!),
                "0/15 * * * * *");

        return app;
    }

    public static IApplicationBuilder UseHangfireDashboard(this IApplicationBuilder app, IConfiguration configuration)
    {
        Dashboard configureDashboard = configuration.GetSection("HangfireSettings:Dashboard").Get<Dashboard>();
        HangfireSettings hangfireSettings = configuration.GetSection("HangfireSettings").Get<HangfireSettings>();
        string hangfireRoute = hangfireSettings?.Route;

        app.UseHangfireDashboard(hangfireRoute, new DashboardOptions
        {
            // Authorization = new [] { },
            DashboardTitle = configureDashboard?.DashboardTitle,
            StatsPollingInterval = configureDashboard?.StatsPollingInterval ?? 0,
            AppPath = configureDashboard?.AppPath,
            IgnoreAntiforgeryToken = true,
        });

        return app;
    }
}

using EventHub.Infrastructure.Outbox;
using EventHub.Shared.Settings;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace EventHub.Infrastructure.Configurations;

public static class QuartzConfiguration
{
    public static IServiceCollection ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            const string jobName = nameof(ProcessOutboxMessagesJob);

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobConfigure => jobConfigure.WithIdentity(jobName))
                .AddTrigger(triggerConfigure =>
                    triggerConfigure
                        .ForJob(jobName)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(10).RepeatForever()));
        });

        services.AddQuartz();

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}
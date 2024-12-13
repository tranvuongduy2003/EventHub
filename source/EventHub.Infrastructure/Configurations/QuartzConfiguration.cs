using EventHub.Application.SeedWork.Abstractions;
using EventHub.Infrastructure.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Simpl;

namespace EventHub.Infrastructure.Configurations;

public static class QuartzConfiguration
{
    public static IServiceCollection ConfigureQuartz(this IServiceCollection services)
    {
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey)
                .AddTrigger(triggerConfigure =>
                    triggerConfigure
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                            schedule.WithIntervalInSeconds(5).RepeatForever()));
            
            configure.UseJobFactory<MicrosoftDependencyInjectionJobFactory>();
        });

        services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

        return services;
    }
}

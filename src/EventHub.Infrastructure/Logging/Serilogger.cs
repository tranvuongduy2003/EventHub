using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EventHub.Infrastructure.Logging;

public static class Serilogger
{
    public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
        (context, configuration) =>
        {
            var environmentName = context.HostingEnvironment.EnvironmentName ?? "Development";
            var serverUrl = context.Configuration.GetValue<string>("SeqConfiguration:ServerUrl") ?? "";

            configuration
                .WriteTo.Debug()
                .WriteTo.Seq(serverUrl)
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithProperty("Environment", environmentName)
                .ReadFrom.Configuration(context.Configuration);
        };
}
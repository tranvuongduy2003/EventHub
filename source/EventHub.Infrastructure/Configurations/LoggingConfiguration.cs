using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace EventHub.Infrastructure.Configurations;

/// <summary>
/// Provides a static configuration setup for Serilog logging based on the host environment and application configuration.
/// </summary>
public static class LoggingConfiguration
{
    /// <summary>
    /// Gets an <see cref="Action{HostBuilderContext, LoggerConfiguration}"/> delegate for configuring Serilog.
    /// </summary>
    /// <remarks>
    /// The delegate configures Serilog with:
    /// <list type="bullet">
    /// <item><description>Debug output sink.</description></item>
    /// <item><description>Seq output sink, with the server URL specified in the configuration.</description></item>
    /// <item><description>Console output sink with a custom output template.</description></item>
    /// <item><description>Enrichment from the log context.</description></item>
    /// <item><description>Machine name enrichment.</description></item>
    /// <item><description>Environment name enrichment, derived from the hosting environment.</description></item>
    /// <item><description>Configuration settings from the application's configuration.</description></item>
    /// </list>
    /// </remarks>
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
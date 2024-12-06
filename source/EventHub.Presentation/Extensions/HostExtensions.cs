namespace EventHub.Presentation.Extensions;

internal static class HostExtensions
{
    public static void AddAppConfigurations(this WebApplicationBuilder builder)
    {
        string environment = builder.Environment.EnvironmentName;

        builder.Configuration
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{environment}.json", true, true)
                .AddEnvironmentVariables();
    }
}

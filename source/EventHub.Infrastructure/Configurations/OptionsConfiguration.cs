using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class OptionsConfiguration
{
    /// <summary>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services"></param>
    /// <param name="sectionName"></param>
    /// <returns></returns>
    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
        IConfigurationSection section = configuration.GetSection(sectionName);
        var options = new T();
        section.Bind(options);

        return options;
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Configurations;

public static class HttpClientConfiguration
{
    public static IServiceCollection ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        string sentimentAnalysisUrl = configuration.GetValue<string>("ServiceUrls:SentimentAnalysis");
        if (sentimentAnalysisUrl == null || string.IsNullOrEmpty(sentimentAnalysisUrl))
        {
            throw new NullReferenceException("SentimentAnalysis is not configured.");
        }
        services.AddHttpClient("SentimentAnalysis",
                u => u.BaseAddress = new Uri(sentimentAnalysisUrl));

        string recommenderSystemUrl = configuration.GetValue<string>("ServiceUrls:RecommenderSystem");
        if (recommenderSystemUrl == null || string.IsNullOrEmpty(recommenderSystemUrl))
        {
            throw new NullReferenceException("RecommenderSystem is not configured.");
        }
        services.AddHttpClient("RecommenderSystem",
                u => u.BaseAddress = new Uri(recommenderSystemUrl));

        return services;
    }
}

using EventHub.Application.Hubs;
using EventHub.Infrastructure.Configurations;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Presentation.Middlewares;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Presentation.Extensions;

internal static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app, string appCors)
    {
        app.UseSwaggerDocumentation();

        // 1, Exception Handler
        app.UseMiddleware<ErrorWrappingMiddleware>();

        // 2, HSTS

        // 3, HttpsRedirection
        if (app.Environment.IsProduction())
        {
            app.UseHttpsRedirection(); //production only
        }

        // 4, Static Files

        // 5, Routing

        // 6, CORS
        app.UseCors(appCors);

        // 7, Authentication
        app.UseAuthentication();

        // 8, Authorization
        app.UseAuthorization();

        // 9, Custom
        app.UseHangfireDashboard(app.Configuration);
        app.MapGet("/", context => Task.Run(() =>
            context.Response.Redirect("/swagger/index.html")));

        app.MapHub<ChatHub>("/chat");
        app.MapHub<NotificationHub>("/notification");

        // 10, Endpoint
        app.MapControllers();

        // Auto migrating and seeding data
        using (IServiceScope scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;
            ILogger<ApplicationDbContext> logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

            try
            {
                logger.LogInformation("Migrating database.");

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                logger.LogInformation("Migrated database.");
                logger.LogInformation("Seeding data...");

                ApplicationDbContextSeed seeder = services.GetService<ApplicationDbContextSeed>();
                seeder?.Seed().Wait();

                logger.LogInformation("Seeding data successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}

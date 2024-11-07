using EventHub.Infrastructure.Configurations;
using EventHub.Persistence.Data;
using EventHub.Presentation.Middlewares;
using EventHub.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EventHub.Presentation.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app, string appCors)
    {
        app.UseSwaggerDocumentation();

        // 1, Exception Handler
        app.UseMiddleware<ErrorWrappingMiddleware>();

        // 2, HSTS

        // 3, HttpsRedirection
        app.UseHttpsRedirection(); //production only

        // 4, Static Files

        // 5, Routing

        // 6, CORS
        app.UseCors(appCors);

        // 7, Authentication
        app.UseAuthentication();

        // 8, Authorization
        app.UseAuthorization();

        // 9, Custom
        app.UseHangfireBackgroundJobs();
        app.UseHangfireDashboard(app.Configuration);
        app.MapGet("/", context => Task.Run(() =>
            context.Response.Redirect("/swagger/index.html")));
        app.MapHub<ChatHub>("/Chat");

        // 10, Endpoint
        app.MapControllers();

        // Auto migrating and seeding data
        using var scope = app.Services.CreateScope();
        {
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            try
            {
                logger.LogInformation("Migrating database.");
                if (context.Database.GetPendingMigrations().Any())
                    context.Database.Migrate();
                logger.LogInformation("Migrated database.");

                Log.Information("Seeding data...");
                var dbInitializer = services.GetService<ApplicationDbContextSeed>();
                dbInitializer?.Seed().Wait();
                Log.Information("Seeding data successfully!");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
            }
        }
    }
}
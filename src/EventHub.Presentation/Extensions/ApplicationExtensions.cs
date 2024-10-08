﻿using EventHub.Infrastructure.Configurations;
using EventHub.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EventHub.Presentation.Extensions;

public static class ApplicationExtensions
{
    public static void UseInfrastructure(this WebApplication app, string appCors)
    {
        app.UseSwaggerDocumentation();
        
        // app.UseHttpsRedirection(); //production only

        app.UseErrorWrapping();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCors(appCors);
        app.UseHangfireBackgroundJobs();
        app.UseHangfireDashboard(app.Configuration);
        app.MapControllers();
        app.MapGet("/", context => Task.Run(() =>
            context.Response.Redirect("/swagger/index.html")));

        // Hubs
        // app.MapHub<ChatHub>("/Chat");

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
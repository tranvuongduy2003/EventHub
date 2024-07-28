using EventHub.Domain.Common.Repository;
using EventHub.Domain.Common.UnitOfWork;
using EventHub.Domain.Contracts;
using EventHub.Domain.Interfaces;
using EventHub.Infrastructor.Caching;
using EventHub.Infrastructor.Common.Repository;
using EventHub.Infrastructor.Common.UnitOfWork;
using EventHub.Infrastructor.Configurations;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.FilesSystem;
using EventHub.Infrastructor.Hangfire;
using EventHub.Infrastructor.Mailler;
using EventHub.Infrastructor.Repositories;
using EventHub.Infrastructor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IHangfireService = EventHub.Domain.Hangfire.IHangfireService;

namespace EventHub.Infrastructor;

public static class DependencyInjection
{
    public static IServiceCollection ConfigureInfrastructorServices(this IServiceCollection services,
        IConfiguration configuration, string appCors)
    {
        services.ConfigureRedis(configuration);
        services.ConfigureControllers();
        services.ConfigureCors(appCors);
        services.ConfigureApplicationDbContext(configuration);
        services.ConfigureHangfireServices();
        services.ConfigureIdentity();
        services.ConfigureMapper();
        services.ConfigValidation();
        services.ConfigureSwagger();
        services.ConfigureAppSettings(configuration);
        services.ConfigureApplication();
        services.ConfigureAzureSignalR(configuration);
        services.ConfigureAuthetication();
        services.ConfigureDependencyInjection();
        services.ConfigureRepositories();

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        return services
            .AddTransient<ApplicationDbContextSeed>()
            .AddTransient<ISerializeService, SerializeService>()
            .AddTransient<ICacheService, CacheService>()
            .AddTransient<IBlobService, AzureBlobService>()
            .AddTransient<IHangfireService, HangfireService>()
            .AddTransient<IEmailService, EmailService>()
            .AddTransient<ITokenService, TokenService>();

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddScoped<ICategoriesRepository, CategoriesRepository>()
            .AddScoped<ICommandInFunctionsRepository, CommandInFunctionsRepository>()
            .AddScoped<ICommandsRepository, CommandsRepository>()
            .AddScoped<IConversationsRepository, ConversationsRepository>()
            .AddScoped<IEmailAttachmentsRepository, EmailAttachmentsRepository>()
            .AddScoped<IEmailContentsRepository, EmailContentsRepository>()
            .AddScoped<IEmailLoggersRepository, EmailLoggersRepository>()
            .AddScoped<IEventCategoriesRepository, EventCategoriesRepository>()
            .AddScoped<IEventsRepository, EventsRepository>()
            .AddScoped<IEventSubImagesRepository, EventSubImagesRepository>()
            .AddScoped<IFavouriteEventsRepository, FavouriteEventsRepository>()
            .AddScoped<IFunctionsRepository, FunctionsRepository>()
            .AddScoped<IInvitationsRepository, InvitationsRepository>()
            .AddScoped<ILabelInEventsRepository, LabelInEventsRepository>()
            .AddScoped<ILabelInUsersRepository, LabelInUsersRepository>()
            .AddScoped<ILabelsRepository, LabelsRepository>()
            .AddScoped<IMessagesRepository, MessagesRepository>()
            .AddScoped<IPaymentItemsRepository, PaymentItemsRepository>()
            .AddScoped<IPaymentMethodsRepository, PaymentMethodsRepository>()
            .AddScoped<IPaymentsRepository, PaymentsRepository>()
            .AddScoped<IPermissionsRepository, PermissionsRepository>()
            .AddScoped<IReasonsRepository, ReasonsRepository>()
            .AddScoped<IReviewsRepository, ReviewsRepository>()
            .AddScoped<ITicketsRepository, TicketsRepository>()
            .AddScoped<ITicketTypesRepository, TicketTypesRepository>()
            .AddScoped<IUserFollowersRepository, UserFollowersRepository>()
            .AddScoped<IUserPaymentMethodsRepository, UserPaymentMethodsRepository>();

        return services;
    }
}
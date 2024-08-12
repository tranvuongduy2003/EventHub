using EventHub.Domain.AggregateModels.CategoryAggregate;
using EventHub.Domain.AggregateModels.ConversationAggregate;
using EventHub.Domain.AggregateModels.EmailLoggerAggregate;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.AggregateModels.LabelAggregate;
using EventHub.Domain.AggregateModels.PaymentAggregate;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.AggregateModels.ReviewAggregate;
using EventHub.Domain.AggregateModels.TicketAggregate;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Domain.Services;
using EventHub.Infrastructor.Caching;
using EventHub.Infrastructor.Configurations;
using EventHub.Infrastructor.Data;
using EventHub.Infrastructor.FilesSystem;
using EventHub.Infrastructor.Hangfire;
using EventHub.Infrastructor.Mailler;
using EventHub.Infrastructor.Repositories;
using EventHub.Infrastructor.SeedWork.Repository;
using EventHub.Infrastructor.SeedWork.UnitOfWork;
using EventHub.Infrastructor.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IHangfireService = EventHub.Domain.Services.IHangfireService;

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
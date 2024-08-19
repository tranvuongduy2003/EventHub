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
using EventHub.Domain.Repositories;
using EventHub.Domain.SeedWork.Repository;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Persistence.CachedRepositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.Repositories;
using EventHub.Persistence.SeedWork.Repository;
using EventHub.Persistence.SeedWork.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureDependencyInjection();
        services.ConfigureRepositories();
        services.ConfigureCachedRepositories();

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        return services
            .AddTransient<ApplicationDbContextSeed>();

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
    
    public static IServiceCollection ConfigureCachedRepositories(this IServiceCollection services)
    {
        return services
            .AddScoped<CachedCategoriesRepository>()
            .AddScoped<CachedEventsRepository>()
            .AddScoped<CachedEventSubImagesRepository>()
            .AddScoped<CachedReasonsRepository>()
            .AddScoped<CachedReviewsRepository>()
            .AddScoped<CachedTicketTypesRepository>();

        return services;
    }
}
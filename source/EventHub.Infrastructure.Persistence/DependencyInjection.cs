using EventHub.Domain.CachedRepositories;
using EventHub.Domain.Repositories;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Infrastructure.Persistence.CachedRepositories;
using EventHub.Infrastructure.Persistence.Data;
using EventHub.Infrastructure.Persistence.Repositories;
using EventHub.Infrastructure.Persistence.SeedWork.Repository;
using EventHub.Infrastructure.Persistence.SeedWork.SqlConnection;
using EventHub.Infrastructure.Persistence.SeedWork.UnitOfWork;
using Microsoft.Extensions.DependencyInjection;

namespace EventHub.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services)
    {
        services.ConfigureDependencyInjection();
        services.ConfigureRepositories();

        return services;
    }

    public static IServiceCollection ConfigureDependencyInjection(this IServiceCollection services)
    {
        services
            .AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();

        services
            .AddTransient<ApplicationDbContextSeed>();

        return services;
    }

    public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
    {
        services
            .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
            .AddScoped(typeof(ICachedRepositoryBase<>), typeof(CachedRepositoryBase<>))
            .AddScoped<IUnitOfWork, UnitOfWork>();

        services
            .AddScoped<ICategoriesRepository, CategoriesRepository>()
            .AddScoped<ICommandInFunctionsRepository, CommandInFunctionsRepository>()
            .AddScoped<ICommandsRepository, CommandsRepository>()
            .AddScoped<IConversationsRepository, ConversationsRepository>()
            .AddScoped<IEmailAttachmentsRepository, EmailAttachmentsRepository>()
            .AddScoped<IEmailContentsRepository, EmailContentsRepository>()
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

        services
            .AddScoped<ICachedCategoriesRepository, CachedCategoriesRepository>()
            .AddScoped<ICachedEventsRepository, CachedEventsRepository>()
            .AddScoped<ICachedEventSubImagesRepository, CachedEventSubImagesRepository>()
            .AddScoped<ICachedReasonsRepository, CachedReasonsRepository>()
            .AddScoped<ICachedReviewsRepository, CachedReviewsRepository>()
            .AddScoped<ICachedTicketTypesRepository, CachedTicketTypesRepository>();

        return services;
    }
}

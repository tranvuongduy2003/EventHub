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
using EventHub.Domain.CachedRepositories;
using EventHub.Domain.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Domain.SeedWork.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    ICategoriesRepository Categories { get; }
    ICommandInFunctionsRepository CommandInFunctions { get; }
    ICommandsRepository Commands { get; }
    IConversationsRepository Conversations { get; }
    IEmailAttachmentsRepository EmailAttachments { get; }
    IEmailContentsRepository EmailContents { get; }
    IEmailLoggersRepository EmailLoggers { get; }
    IEventCategoriesRepository EventCategories { get; }
    IEventsRepository Events { get; }
    IEventSubImagesRepository EventSubImages { get; }
    IFavouriteEventsRepository FavouriteEvents { get; }
    IFunctionsRepository Functions { get; }
    IInvitationsRepository Invitations { get; }
    ILabelInEventsRepository LabelInEvents { get; }
    ILabelInUsersRepository LabelInUsers { get; }
    ILabelsRepository Labels { get; }
    IMessagesRepository Messages { get; }
    IPaymentItemsRepository PaymentItems { get; }
    IPaymentMethodsRepository PaymentMethods { get; }
    IPaymentsRepository Payments { get; }
    IPermissionsRepository Permissions { get; }
    IReasonsRepository Reasons { get; }
    IReviewsRepository Reviews { get; }
    ITicketsRepository Tickets { get; }
    ITicketTypesRepository TicketTypes { get; }
    IUserFollowersRepository UserFollowers { get; }
    IUserPaymentMethodsRepository UserPaymentMethods { get; }

    ICachedCategoriesRepository CachedCategories { get; }
    ICachedEventsRepository CachedEvents { get; }
    ICachedEventSubImagesRepository CachedEventSubImages { get; }
    ICachedReasonsRepository CachedReasons { get; }
    ICachedReviewsRepository CachedReviews { get; }
    ICachedTicketTypesRepository CachedTicketTypes { get; }

    void Dispose();

    Task<int> CommitAsync();

    Task<IDbContextTransaction> BeginTransactionAsync();

    Task EndTransactionAsync();

    Task RollbackTransactionAsync();
}
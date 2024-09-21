using EventHub.Abstractions.CachedRepositories;
using EventHub.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Abstractions.SeedWork.UnitOfWork;

/// <summary>
/// Defines the contract for a unit of work that manages database operations and caching.
/// </summary>
/// <remarks>
/// The Unit of Work pattern coordinates the work of multiple repositories and handles transactions. It also integrates
/// with caching services to optimize data access. This interface provides access to various repositories and methods
/// for managing database transactions and caching.
/// </remarks>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Commits the current transaction to the database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the number of affected rows.</returns>
    Task<int> CommitAsync();

    /// <summary>
    /// Begins a new database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation. The task result contains the transaction object.</returns>
    Task<IDbContextTransaction> BeginTransactionAsync();

    /// <summary>
    /// Ends the current database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task EndTransactionAsync();

    /// <summary>
    /// Rolls back the current database transaction asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task RollbackTransactionAsync();

    #region Repository Properties

    /// <summary>
    /// Gets the repository for managing categories.
    /// </summary>
    ICategoriesRepository Categories { get; }

    /// <summary>
    /// Gets the repository for managing commands in functions.
    /// </summary>
    ICommandInFunctionsRepository CommandInFunctions { get; }

    /// <summary>
    /// Gets the repository for managing commands.
    /// </summary>
    ICommandsRepository Commands { get; }

    /// <summary>
    /// Gets the repository for managing conversations.
    /// </summary>
    IConversationsRepository Conversations { get; }

    /// <summary>
    /// Gets the repository for managing email attachments.
    /// </summary>
    IEmailAttachmentsRepository EmailAttachments { get; }

    /// <summary>
    /// Gets the repository for managing email contents.
    /// </summary>
    IEmailContentsRepository EmailContents { get; }

    /// <summary>
    /// Gets the repository for managing email loggers.
    /// </summary>
    IEmailLoggersRepository EmailLoggers { get; }

    /// <summary>
    /// Gets the repository for managing event categories.
    /// </summary>
    IEventCategoriesRepository EventCategories { get; }

    /// <summary>
    /// Gets the repository for managing events.
    /// </summary>
    IEventsRepository Events { get; }

    /// <summary>
    /// Gets the repository for managing event sub-images.
    /// </summary>
    IEventSubImagesRepository EventSubImages { get; }

    /// <summary>
    /// Gets the repository for managing favourite events.
    /// </summary>
    IFavouriteEventsRepository FavouriteEvents { get; }

    /// <summary>
    /// Gets the repository for managing functions.
    /// </summary>
    IFunctionsRepository Functions { get; }

    /// <summary>
    /// Gets the repository for managing invitations.
    /// </summary>
    IInvitationsRepository Invitations { get; }

    /// <summary>
    /// Gets the repository for managing labels in events.
    /// </summary>
    ILabelInEventsRepository LabelInEvents { get; }

    /// <summary>
    /// Gets the repository for managing labels in users.
    /// </summary>
    ILabelInUsersRepository LabelInUsers { get; }

    /// <summary>
    /// Gets the repository for managing labels.
    /// </summary>
    ILabelsRepository Labels { get; }

    /// <summary>
    /// Gets the repository for managing messages.
    /// </summary>
    IMessagesRepository Messages { get; }

    /// <summary>
    /// Gets the repository for managing payment items.
    /// </summary>
    IPaymentItemsRepository PaymentItems { get; }

    /// <summary>
    /// Gets the repository for managing payment methods.
    /// </summary>
    IPaymentMethodsRepository PaymentMethods { get; }

    /// <summary>
    /// Gets the repository for managing payments.
    /// </summary>
    IPaymentsRepository Payments { get; }

    /// <summary>
    /// Gets the repository for managing permissions.
    /// </summary>
    IPermissionsRepository Permissions { get; }

    /// <summary>
    /// Gets the repository for managing reasons.
    /// </summary>
    IReasonsRepository Reasons { get; }

    /// <summary>
    /// Gets the repository for managing reviews.
    /// </summary>
    IReviewsRepository Reviews { get; }

    /// <summary>
    /// Gets the repository for managing tickets.
    /// </summary>
    ITicketsRepository Tickets { get; }

    /// <summary>
    /// Gets the repository for managing ticket types.
    /// </summary>
    ITicketTypesRepository TicketTypes { get; }

    /// <summary>
    /// Gets the repository for managing user followers.
    /// </summary>
    IUserFollowersRepository UserFollowers { get; }

    /// <summary>
    /// Gets the repository for managing user payment methods.
    /// </summary>
    IUserPaymentMethodsRepository UserPaymentMethods { get; }

    #region Caching Repository Properties

    /// <summary>
    /// Gets the repository for managing cached categories.
    /// </summary>
    ICachedCategoriesRepository CachedCategories { get; }

    /// <summary>
    /// Gets the repository for managing cached events.
    /// </summary>
    ICachedEventsRepository CachedEvents { get; }

    /// <summary>
    /// Gets the repository for managing cached event sub-images.
    /// </summary>
    ICachedEventSubImagesRepository CachedEventSubImages { get; }

    /// <summary>
    /// Gets the repository for managing cached reasons.
    /// </summary>
    ICachedReasonsRepository CachedReasons { get; }

    /// <summary>
    /// Gets the repository for managing cached reviews.
    /// </summary>
    ICachedReviewsRepository CachedReviews { get; }

    /// <summary>
    /// Gets the repository for managing cached ticket types.
    /// </summary>
    ICachedTicketTypesRepository CachedTicketTypes { get; }

    #endregion

    #endregion
}
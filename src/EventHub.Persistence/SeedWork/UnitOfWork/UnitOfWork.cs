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
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Domain.Services;
using EventHub.Persistence.CachedRepositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Persistence.SeedWork.UnitOfWork;

/// <summary>
/// Represents a unit of work pattern implementation, managing database operations and caching.
/// </summary>
/// <remarks>
/// The Unit of Work pattern is used to handle transactions across multiple repositories and coordinate changes
/// in a single transaction. This class also integrates with a caching service to optimize performance and reduce
/// database load.
/// </remarks>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private bool _disposed;

    private CategoriesRepository _categories;
    private CommandInFunctionsRepository _commandInFunctions;
    private CommandsRepository _commands;
    private ConversationsRepository _conversations;
    private EmailAttachmentsRepository _emailAttachments;
    private EmailContentsRepository _emailContents;
    private EmailLoggersRepository _emailLoggers;
    private EventCategoriesRepository _eventCategories;
    private EventsRepository _events;
    private EventSubImagesRepository _eventSubImages;
    private FavouriteEventsRepository _favouriteEvents;
    private FunctionsRepository _functions;
    private InvitationsRepository _invitations;
    private LabelInEventsRepository _labelInEvents;
    private LabelInUsersRepository _labelInUsers;
    private LabelsRepository _labels;
    private MessagesRepository _messages;
    private PaymentItemsRepository _paymentItems;
    private PaymentMethodsRepository _paymentMethods;
    private PaymentsRepository _payments;
    private PermissionsRepository _permissions;
    private ReviewsRepository _reviews;
    private ReasonsRepository _reasons;
    private TicketsRepository _tickets;
    private TicketTypesRepository _ticketTypes;
    private UserFollowersRepository _userFollowers;
    private UserPaymentMethodsRepository _userPaymentMethods;

    private CachedCategoriesRepository _cachedCategories;
    private CachedEventsRepository _cachedEvents;
    private CachedEventSubImagesRepository _cachedEventSubImages;
    private CachedReasonsRepository _cachedReasons;
    private CachedReviewsRepository _cachedReviews;
    private CachedTicketTypesRepository _cachedTicketTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="context">The database context used for database operations.</param>
    /// <param name="cacheService">The caching service used to cache data and reduce database load.</param>
    public UnitOfWork(ApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public ICategoriesRepository Categories
    {
        get
        {
            if (_categories == null)
                _categories = new CategoriesRepository(_context);
            return _categories;
        }
    }

    public ICommandInFunctionsRepository CommandInFunctions
    {
        get
        {
            if (_commandInFunctions == null)
                _commandInFunctions = new CommandInFunctionsRepository(_context);
            return _commandInFunctions;
        }
    }

    public ICommandsRepository Commands
    {
        get
        {
            if (_commands == null)
                _commands = new CommandsRepository(_context);
            return _commands;
        }
    }

    public IConversationsRepository Conversations
    {
        get
        {
            if (_conversations == null)
                _conversations = new ConversationsRepository(_context);
            return _conversations;
        }
    }

    public IEmailAttachmentsRepository EmailAttachments
    {
        get
        {
            if (_emailAttachments == null)
                _emailAttachments = new EmailAttachmentsRepository(_context);
            return _emailAttachments;
        }
    }

    public IEmailContentsRepository EmailContents
    {
        get
        {
            if (_emailContents == null)
                _emailContents = new EmailContentsRepository(_context);
            return _emailContents;
        }
    }

    public IEmailLoggersRepository EmailLoggers
    {
        get
        {
            if (_emailLoggers == null)
                _emailLoggers = new EmailLoggersRepository(_context);
            return _emailLoggers;
        }
    }

    public IEventCategoriesRepository EventCategories
    {
        get
        {
            if (_eventCategories == null)
                _eventCategories = new EventCategoriesRepository(_context);
            return _eventCategories;
        }
    }

    public IEventsRepository Events
    {
        get
        {
            if (_events == null)
                _events = new EventsRepository(_context);
            return _events;
        }
    }

    public IEventSubImagesRepository EventSubImages
    {
        get
        {
            if (_eventSubImages == null)
                _eventSubImages = new EventSubImagesRepository(_context);
            return _eventSubImages;
        }
    }

    public IFavouriteEventsRepository FavouriteEvents
    {
        get
        {
            if (_favouriteEvents == null)
                _favouriteEvents = new FavouriteEventsRepository(_context);
            return _favouriteEvents;
        }
    }

    public IFunctionsRepository Functions
    {
        get
        {
            if (_functions == null)
                _functions = new FunctionsRepository(_context);
            return _functions;
        }
    }

    public IInvitationsRepository Invitations
    {
        get
        {
            if (_invitations == null)
                _invitations = new InvitationsRepository(_context);
            return _invitations;
        }
    }

    public ILabelInEventsRepository LabelInEvents
    {
        get
        {
            if (_labelInEvents == null)
                _labelInEvents = new LabelInEventsRepository(_context);
            return _labelInEvents;
        }
    }

    public ILabelInUsersRepository LabelInUsers
    {
        get
        {
            if (_labelInUsers == null)
                _labelInUsers = new LabelInUsersRepository(_context);
            return _labelInUsers;
        }
    }

    public ILabelsRepository Labels
    {
        get
        {
            if (_labels == null)
                _labels = new LabelsRepository(_context);
            return _labels;
        }
    }

    public IMessagesRepository Messages
    {
        get
        {
            if (_messages == null)
                _messages = new MessagesRepository(_context);
            return _messages;
        }
    }

    public IPaymentItemsRepository PaymentItems
    {
        get
        {
            if (_paymentItems == null)
                _paymentItems = new PaymentItemsRepository(_context);
            return _paymentItems;
        }
    }

    public IPaymentMethodsRepository PaymentMethods
    {
        get
        {
            if (_paymentMethods == null)
                _paymentMethods = new PaymentMethodsRepository(_context);
            return _paymentMethods;
        }
    }

    public IPaymentsRepository Payments
    {
        get
        {
            if (_payments == null)
                _payments = new PaymentsRepository(_context);
            return _payments;
        }
    }

    public IPermissionsRepository Permissions
    {
        get
        {
            if (_permissions == null)
                _permissions = new PermissionsRepository(_context);
            return _permissions;
        }
    }

    public IReasonsRepository Reasons
    {
        get
        {
            if (_reasons == null)
                _reasons = new ReasonsRepository(_context);
            return _reasons;
        }
    }

    public IReviewsRepository Reviews
    {
        get
        {
            if (_reviews == null)
                _reviews = new ReviewsRepository(_context);
            return _reviews;
        }
    }

    public ITicketsRepository Tickets
    {
        get
        {
            if (_tickets == null)
                _tickets = new TicketsRepository(_context);
            return _tickets;
        }
    }

    public ITicketTypesRepository TicketTypes
    {
        get
        {
            if (_ticketTypes == null)
                _ticketTypes = new TicketTypesRepository(_context);
            return _ticketTypes;
        }
    }

    public IUserFollowersRepository UserFollowers
    {
        get
        {
            if (_userFollowers == null)
                _userFollowers = new UserFollowersRepository(_context);
            return _userFollowers;
        }
    }

    public IUserPaymentMethodsRepository UserPaymentMethods
    {
        get
        {
            if (_userPaymentMethods == null)
                _userPaymentMethods = new UserPaymentMethodsRepository(_context);
            return _userPaymentMethods;
        }
    }

    public ICachedCategoriesRepository CachedCategories
    {
        get
        {
            if (_categories == null)
                _categories = new CategoriesRepository(_context);

            if (_cachedCategories == null)
                _cachedCategories =
                    new CachedCategoriesRepository(_context, _categories, _cacheService);
            return _cachedCategories;
        }
    }

    public ICachedEventsRepository CachedEvents
    {
        get
        {
            if (_events == null)
                _events = new EventsRepository(_context);

            if (_cachedEvents == null)
                _cachedEvents = new CachedEventsRepository(_context, _events, _cacheService);
            return _cachedEvents;
        }
    }

    public ICachedEventSubImagesRepository CachedEventSubImages
    {
        get
        {
            if (_eventSubImages == null)
                _eventSubImages = new EventSubImagesRepository(_context);

            if (_cachedEventSubImages == null)
                _cachedEventSubImages =
                    new CachedEventSubImagesRepository(_context, _eventSubImages, _cacheService);
            return _cachedEventSubImages;
        }
    }

    public ICachedReasonsRepository CachedReasons
    {
        get
        {
            if (_reasons == null)
                _reasons = new ReasonsRepository(_context);

            if (_cachedReasons == null)
                _cachedReasons = new CachedReasonsRepository(_context, _reasons, _cacheService);
            return _cachedReasons;
        }
    }

    public ICachedReviewsRepository CachedReviews
    {
        get
        {
            if (_reviews == null)
                _reviews = new ReviewsRepository(_context);

            if (_cachedReviews == null)
                _cachedReviews = new CachedReviewsRepository(_context, _reviews, _cacheService);
            return _cachedReviews;
        }
    }

    public ICachedTicketTypesRepository CachedTicketTypes
    {
        get
        {
            if (_ticketTypes == null)
                _ticketTypes = new TicketTypesRepository(_context);

            if (_cachedTicketTypes == null)
                _cachedTicketTypes =
                    new CachedTicketTypesRepository(_context, _ticketTypes, _cacheService);
            return _cachedTicketTypes;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }

    public Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return _context.Database.BeginTransactionAsync();
    }

    public async Task EndTransactionAsync()
    {
        await CommitAsync();
        await _context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return _context.Database.RollbackTransactionAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();
        _disposed = true;
    }
}
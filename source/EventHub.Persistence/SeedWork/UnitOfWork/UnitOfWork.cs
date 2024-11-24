﻿using EventHub.Abstractions.CachedRepositories;
using EventHub.Abstractions.Repositories;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Abstractions.Services;
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
    private readonly ICacheService _cacheService;
    private readonly ApplicationDbContext _context;

    private CachedCategoriesRepository _cachedCategories;
    private CachedEventsRepository _cachedEvents;
    private CachedEventSubImagesRepository _cachedEventSubImages;
    private CachedReasonsRepository _cachedReasons;
    private CachedReviewsRepository _cachedReviews;
    private CachedTicketTypesRepository _cachedTicketTypes;

    private CategoriesRepository _categories;
    private CommandInFunctionsRepository _commandInFunctions;
    private CommandsRepository _commands;
    private ConversationsRepository _conversations;
    private bool _disposed;
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
    private ReasonsRepository _reasons;
    private ReviewsRepository _reviews;
    private TicketsRepository _tickets;
    private TicketTypesRepository _ticketTypes;
    private UserFollowersRepository _userFollowers;
    private UserPaymentMethodsRepository _userPaymentMethods;

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
            _categories ??= new CategoriesRepository(_context);
            return _categories;
        }
    }

    public ICommandInFunctionsRepository CommandInFunctions
    {
        get
        {
            _commandInFunctions ??= new CommandInFunctionsRepository(_context);
            return _commandInFunctions;
        }
    }

    public ICommandsRepository Commands
    {
        get
        {
            _commands ??= new CommandsRepository(_context);
            return _commands;
        }
    }

    public IConversationsRepository Conversations
    {
        get
        {
            _conversations ??= new ConversationsRepository(_context);
            return _conversations;
        }
    }

    public IEmailAttachmentsRepository EmailAttachments
    {
        get
        {
            _emailAttachments ??= new EmailAttachmentsRepository(_context);
            return _emailAttachments;
        }
    }

    public IEmailContentsRepository EmailContents
    {
        get
        {
            _emailContents ??= new EmailContentsRepository(_context);
            return _emailContents;
        }
    }

    public IEmailLoggersRepository EmailLoggers
    {
        get
        {
            _emailLoggers ??= new EmailLoggersRepository(_context);
            return _emailLoggers;
        }
    }

    public IEventCategoriesRepository EventCategories
    {
        get
        {
            _eventCategories ??= new EventCategoriesRepository(_context);
            return _eventCategories;
        }
    }

    public IEventsRepository Events
    {
        get
        {
            _events ??= new EventsRepository(_context);
            return _events;
        }
    }

    public IEventSubImagesRepository EventSubImages
    {
        get
        {
            _eventSubImages ??= new EventSubImagesRepository(_context);
            return _eventSubImages;
        }
    }

    public IFavouriteEventsRepository FavouriteEvents
    {
        get
        {
            _favouriteEvents ??= new FavouriteEventsRepository(_context);
            return _favouriteEvents;
        }
    }

    public IFunctionsRepository Functions
    {
        get
        {
            _functions ??= new FunctionsRepository(_context);
            return _functions;
        }
    }

    public IInvitationsRepository Invitations
    {
        get
        {
            _invitations ??= new InvitationsRepository(_context);
            return _invitations;
        }
    }

    public ILabelInEventsRepository LabelInEvents
    {
        get
        {
            _labelInEvents ??= new LabelInEventsRepository(_context);
            return _labelInEvents;
        }
    }

    public ILabelInUsersRepository LabelInUsers
    {
        get
        {
            _labelInUsers ??= new LabelInUsersRepository(_context);
            return _labelInUsers;
        }
    }

    public ILabelsRepository Labels
    {
        get
        {
            _labels ??= new LabelsRepository(_context);
            return _labels;
        }
    }

    public IMessagesRepository Messages
    {
        get
        {
            _messages ??= new MessagesRepository(_context);
            return _messages;
        }
    }

    public IPaymentItemsRepository PaymentItems
    {
        get
        {
            _paymentItems ??= new PaymentItemsRepository(_context);
            return _paymentItems;
        }
    }

    public IPaymentMethodsRepository PaymentMethods
    {
        get
        {
            _paymentMethods ??= new PaymentMethodsRepository(_context);
            return _paymentMethods;
        }
    }

    public IPaymentsRepository Payments
    {
        get
        {
            _payments ??= new PaymentsRepository(_context);
            return _payments;
        }
    }

    public IPermissionsRepository Permissions
    {
        get
        {
            _permissions ??= new PermissionsRepository(_context);
            return _permissions;
        }
    }

    public IReasonsRepository Reasons
    {
        get
        {
            _reasons ??= new ReasonsRepository(_context);
            return _reasons;
        }
    }

    public IReviewsRepository Reviews
    {
        get
        {
            _reviews ??= new ReviewsRepository(_context);
            return _reviews;
        }
    }

    public ITicketsRepository Tickets
    {
        get
        {
            _tickets ??= new TicketsRepository(_context);
            return _tickets;
        }
    }

    public ITicketTypesRepository TicketTypes
    {
        get
        {
            _ticketTypes ??= new TicketTypesRepository(_context);
            return _ticketTypes;
        }
    }

    public IUserFollowersRepository UserFollowers
    {
        get
        {
            _userFollowers ??= new UserFollowersRepository(_context);
            return _userFollowers;
        }
    }

    public IUserPaymentMethodsRepository UserPaymentMethods
    {
        get
        {
            _userPaymentMethods ??= new UserPaymentMethodsRepository(_context);
            return _userPaymentMethods;
        }
    }

    public ICachedCategoriesRepository CachedCategories
    {
        get
        {
            _categories ??= new CategoriesRepository(_context);
            _cachedCategories ??=
                new CachedCategoriesRepository(_context, _categories, _cacheService);
            return _cachedCategories;
        }
    }

    public ICachedEventsRepository CachedEvents
    {
        get
        {
            _events ??= new EventsRepository(_context);
            _cachedEvents ??= new CachedEventsRepository(_context, _events, _cacheService);
            return _cachedEvents;
        }
    }

    public ICachedEventSubImagesRepository CachedEventSubImages
    {
        get
        {
            _eventSubImages ??= new EventSubImagesRepository(_context);
            _cachedEventSubImages ??=
                new CachedEventSubImagesRepository(_context, _eventSubImages, _cacheService);
            return _cachedEventSubImages;
        }
    }

    public ICachedReasonsRepository CachedReasons
    {
        get
        {
            _reasons ??= new ReasonsRepository(_context);
            _cachedReasons ??= new CachedReasonsRepository(_context, _reasons, _cacheService);
            return _cachedReasons;
        }
    }

    public ICachedReviewsRepository CachedReviews
    {
        get
        {
            _reviews ??= new ReviewsRepository(_context);
            _cachedReviews ??= new CachedReviewsRepository(_context, _reviews, _cacheService);
            return _cachedReviews;
        }
    }

    public ICachedTicketTypesRepository CachedTicketTypes
    {
        get
        {
            _ticketTypes ??= new TicketTypesRepository(_context);
            _cachedTicketTypes ??=
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
        if (!_disposed && disposing)
        {
            _context.Dispose();
        }
        _disposed = true;
    }
}
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Domain.Services;
using EventHub.Persistence.CachedRepositories;
using EventHub.Persistence.Data;
using EventHub.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventHub.Persistence.SeedWork.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private readonly ICacheService _cacheService;
    private bool _disposed;

    private CategoriesRepository _categoriesRepository;
    private CommandInFunctionsRepository _commandInFunctionsRepository;
    private CommandsRepository _commandsRepository;
    private ConversationsRepository _conversationsRepository;
    private EmailAttachmentsRepository _emailAttachmentsRepository;
    private EmailContentsRepository _emailContentsRepository;
    private EmailLoggersRepository _emailLoggersRepository;
    private EventCategoriesRepository _eventCategoriesRepository;
    private EventsRepository _eventsRepository;
    private EventSubImagesRepository _eventSubImagesRepository;
    private FavouriteEventsRepository _favouriteEventsRepository;
    private FunctionsRepository _functionsRepository;
    private InvitationsRepository _invitationsRepository;
    private LabelInEventsRepository _labelInEventsRepository;
    private LabelInUsersRepository _labelInUsersRepository;
    private LabelsRepository _labelsRepository;
    private MessagesRepository _messagesRepository;
    private PaymentItemsRepository _paymentItemsRepository;
    private PaymentMethodsRepository _paymentMethodsRepository;
    private PaymentsRepository _paymentsRepository;
    private PermissionsRepository _permissionsRepository;
    private ReviewsRepository _reviewsRepository;
    private ReasonsRepository _reasonsRepository;
    private TicketsRepository _ticketsRepository;
    private TicketTypesRepository _ticketTypesRepository;
    private UserFollowersRepository _userFollowersRepository;
    private UserPaymentMethodsRepository _userPaymentMethodsRepository;

    private CachedCategoriesRepository _cachedCategoriesRepository;
    private CachedEventsRepository _cachedEventsRepository;
    private CachedEventSubImagesRepository _cachedEventSubImagesRepository;
    private CachedReasonsRepository _cachedReasonsRepository;
    private CachedReviewsRepository _cachedReviewsRepository;
    private CachedTicketTypesRepository _cachedTicketTypesRepository;

    public UnitOfWork(ApplicationDbContext context, ICacheService cacheService)
    {
        _context = context;
        _cacheService = cacheService;
    }

    public CategoriesRepository CategoriesRepository
    {
        get
        {
            if (_categoriesRepository == null)
                _categoriesRepository = new CategoriesRepository(_context);
            return _categoriesRepository;
        }
    }

    public CommandInFunctionsRepository CommandInFunctionsRepository
    {
        get
        {
            if (_commandInFunctionsRepository == null)
                _commandInFunctionsRepository = new CommandInFunctionsRepository(_context);
            return _commandInFunctionsRepository;
        }
    }

    public CommandsRepository CommandsRepository
    {
        get
        {
            if (_commandsRepository == null)
                _commandsRepository = new CommandsRepository(_context);
            return _commandsRepository;
        }
    }

    public ConversationsRepository ConversationsRepository
    {
        get
        {
            if (_conversationsRepository == null)
                _conversationsRepository = new ConversationsRepository(_context);
            return _conversationsRepository;
        }
    }

    public EmailAttachmentsRepository EmailAttachmentsRepository
    {
        get
        {
            if (_emailAttachmentsRepository == null)
                _emailAttachmentsRepository = new EmailAttachmentsRepository(_context);
            return _emailAttachmentsRepository;
        }
    }

    public EmailContentsRepository EmailContentsRepository
    {
        get
        {
            if (_emailContentsRepository == null)
                _emailContentsRepository = new EmailContentsRepository(_context);
            return _emailContentsRepository;
        }
    }

    public EmailLoggersRepository EmailLoggersRepository
    {
        get
        {
            if (_emailLoggersRepository == null)
                _emailLoggersRepository = new EmailLoggersRepository(_context);
            return _emailLoggersRepository;
        }
    }

    public EventCategoriesRepository EventCategoriesRepository
    {
        get
        {
            if (_eventCategoriesRepository == null)
                _eventCategoriesRepository = new EventCategoriesRepository(_context);
            return _eventCategoriesRepository;
        }
    }

    public EventsRepository EventsRepository
    {
        get
        {
            if (_eventsRepository == null)
                _eventsRepository = new EventsRepository(_context);
            return _eventsRepository;
        }
    }

    public EventSubImagesRepository EventSubImagesRepository
    {
        get
        {
            if (_eventSubImagesRepository == null)
                _eventSubImagesRepository = new EventSubImagesRepository(_context);
            return _eventSubImagesRepository;
        }
    }

    public FavouriteEventsRepository FavouriteEventsRepository
    {
        get
        {
            if (_favouriteEventsRepository == null)
                _favouriteEventsRepository = new FavouriteEventsRepository(_context);
            return _favouriteEventsRepository;
        }
    }

    public FunctionsRepository FunctionsRepository
    {
        get
        {
            if (_functionsRepository == null)
                _functionsRepository = new FunctionsRepository(_context);
            return _functionsRepository;
        }
    }

    public InvitationsRepository InvitationsRepository
    {
        get
        {
            if (_invitationsRepository == null)
                _invitationsRepository = new InvitationsRepository(_context);
            return _invitationsRepository;
        }
    }

    public LabelInEventsRepository LabelInEventsRepository
    {
        get
        {
            if (_labelInEventsRepository == null)
                _labelInEventsRepository = new LabelInEventsRepository(_context);
            return _labelInEventsRepository;
        }
    }

    public LabelInUsersRepository LabelInUsersRepository
    {
        get
        {
            if (_labelInUsersRepository == null)
                _labelInUsersRepository = new LabelInUsersRepository(_context);
            return _labelInUsersRepository;
        }
    }

    public LabelsRepository LabelsRepository
    {
        get
        {
            if (_labelsRepository == null)
                _labelsRepository = new LabelsRepository(_context);
            return _labelsRepository;
        }
    }

    public MessagesRepository MessagesRepository
    {
        get
        {
            if (_messagesRepository == null)
                _messagesRepository = new MessagesRepository(_context);
            return _messagesRepository;
        }
    }

    public PaymentItemsRepository PaymentItemsRepository
    {
        get
        {
            if (_paymentItemsRepository == null)
                _paymentItemsRepository = new PaymentItemsRepository(_context);
            return _paymentItemsRepository;
        }
    }

    public PaymentMethodsRepository PaymentMethodsRepository
    {
        get
        {
            if (_paymentMethodsRepository == null)
                _paymentMethodsRepository = new PaymentMethodsRepository(_context);
            return _paymentMethodsRepository;
        }
    }

    public PaymentsRepository PaymentsRepository
    {
        get
        {
            if (_paymentsRepository == null)
                _paymentsRepository = new PaymentsRepository(_context);
            return _paymentsRepository;
        }
    }

    public PermissionsRepository PermissionsRepository
    {
        get
        {
            if (_permissionsRepository == null)
                _permissionsRepository = new PermissionsRepository(_context);
            return _permissionsRepository;
        }
    }

    public ReasonsRepository ReasonsRepository
    {
        get
        {
            if (_reasonsRepository == null)
                _reasonsRepository = new ReasonsRepository(_context);
            return _reasonsRepository;
        }
    }

    public ReviewsRepository ReviewsRepository
    {
        get
        {
            if (_reviewsRepository == null)
                _reviewsRepository = new ReviewsRepository(_context);
            return _reviewsRepository;
        }
    }

    public TicketsRepository TicketsRepository
    {
        get
        {
            if (_ticketsRepository == null)
                _ticketsRepository = new TicketsRepository(_context);
            return _ticketsRepository;
        }
    }

    public TicketTypesRepository TicketTypesRepository
    {
        get
        {
            if (_ticketTypesRepository == null)
                _ticketTypesRepository = new TicketTypesRepository(_context);
            return _ticketTypesRepository;
        }
    }

    public UserFollowersRepository UserFollowersRepository
    {
        get
        {
            if (_userFollowersRepository == null)
                _userFollowersRepository = new UserFollowersRepository(_context);
            return _userFollowersRepository;
        }
    }

    public UserPaymentMethodsRepository UserPaymentMethodsRepository
    {
        get
        {
            if (_userPaymentMethodsRepository == null)
                _userPaymentMethodsRepository = new UserPaymentMethodsRepository(_context);
            return _userPaymentMethodsRepository;
        }
    }

    public CachedCategoriesRepository CachedCategoriesRepository
    {
        get
        {
            if (_categoriesRepository == null)
                _categoriesRepository = new CategoriesRepository(_context);
            
            if (_cachedCategoriesRepository == null)
                _cachedCategoriesRepository =
                    new CachedCategoriesRepository(_context, _categoriesRepository, _cacheService);
            return _cachedCategoriesRepository;
        }
    }

    private CachedEventsRepository CachedEventsRepository
    {
        get
        {
            if (_eventsRepository == null)
                _eventsRepository = new EventsRepository(_context);
            
            if (_cachedEventsRepository == null)
                _cachedEventsRepository = new CachedEventsRepository(_context, _eventsRepository, _cacheService);
            return _cachedEventsRepository;
        }
    }

    private CachedEventSubImagesRepository CachedEventSubImagesRepository
    {
        get
        {
            if (_eventSubImagesRepository == null)
                _eventSubImagesRepository = new EventSubImagesRepository(_context);
            
            if (_cachedEventSubImagesRepository == null)
                _cachedEventSubImagesRepository =
                    new CachedEventSubImagesRepository(_context, _eventSubImagesRepository, _cacheService);
            return _cachedEventSubImagesRepository;
        }
    }

    private CachedReasonsRepository CachedReasonsRepository
    {
        get
        {
            if (_reasonsRepository == null)
                _reasonsRepository = new ReasonsRepository(_context);
            
            if (_cachedReasonsRepository == null)
                _cachedReasonsRepository = new CachedReasonsRepository(_context, _reasonsRepository, _cacheService);
            return _cachedReasonsRepository;
        }
    }

    private CachedReviewsRepository CachedReviewsRepository
    {
        get
        {
            if (_reviewsRepository == null)
                _reviewsRepository = new ReviewsRepository(_context);
            
            if (_cachedReviewsRepository == null)
                _cachedReviewsRepository = new CachedReviewsRepository(_context, _reviewsRepository, _cacheService);
            return _cachedReviewsRepository;
        }
    }

    private CachedTicketTypesRepository CachedTicketTypesRepository
    {
        get
        {
            if (_ticketTypesRepository == null)
                _ticketTypesRepository = new TicketTypesRepository(_context);
            
            if (_cachedTicketTypesRepository == null)
                _cachedTicketTypesRepository =
                    new CachedTicketTypesRepository(_context, _ticketTypesRepository, _cacheService);
            return _cachedTicketTypesRepository;
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
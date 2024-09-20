using EventHub.Domain.Abstractions;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEventTicketTypesDomainEventHandler : IDomainEventHandler<DeleteEventTicketTypesDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly ILogger<DeleteEventTicketTypesDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventTicketTypesDomainEventHandler(IUnitOfWork unitOfWork, IFileService fileService,
        ILogger<DeleteEventTicketTypesDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task Handle(DeleteEventTicketTypesDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteEventTicketTypesDomainEventHandler");

        await _unitOfWork.TicketTypes
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ExecuteDeleteAsync();

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: DeleteEventTicketTypesDomainEventHandler");
    }
}
using EventHub.Domain.Abstractions;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEventSubImagesDomainEventHandler : IDomainEventHandler<DeleteEventSubImagesDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly ILogger<DeleteEventSubImagesDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventSubImagesDomainEventHandler(IUnitOfWork unitOfWork, IFileService fileService,
        ILogger<DeleteEventSubImagesDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task Handle(DeleteEventSubImagesDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteEventSubImagesDomainEventHandler");

        var subImages = await _unitOfWork.EventSubImages
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ToListAsync();

        foreach (var image in subImages)
        {
            await _fileService.DeleteAsync($"{FileContainer.EVENTS}/{notification.EventId}", image.ImageFileName);
        }

        await _unitOfWork.EventSubImages.DeleteListAsync(subImages);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: DeleteEventSubImagesDomainEventHandler");
    }
}
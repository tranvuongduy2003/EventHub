using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEventSubImagesDomainEventHandler : IDomainEventHandler<DeleteEventSubImagesDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEventSubImagesDomainEventHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task Handle(DeleteEventSubImagesDomainEvent notification, CancellationToken cancellationToken)
    {
        var subImages = await _unitOfWork.EventSubImages
            .FindByCondition(x => x.EventId.Equals(notification.EventId))
            .ToListAsync();

        foreach (var image in subImages)
        {
            await _fileService.DeleteAsync($"{FileContainer.EVENTS}/{notification.EventId}", image.ImageFileName);
        }

        await _unitOfWork.EventSubImages.DeleteListAsync(subImages);
        await _unitOfWork.CommitAsync();
    }
}
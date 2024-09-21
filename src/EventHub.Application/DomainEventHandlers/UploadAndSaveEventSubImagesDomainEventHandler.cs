using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class UploadAndSaveEventSubImagesDomainEventHandler : IDomainEventHandler<UploadAndSaveEventSubImagesDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly ILogger<UploadAndSaveEventSubImagesDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public UploadAndSaveEventSubImagesDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<UploadAndSaveEventSubImagesDomainEventHandler> logger, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _fileService = fileService;
    }

    public async Task Handle(UploadAndSaveEventSubImagesDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UploadAndSaveEventSubImagesDomainEventHandler");

        var eventSubImages = new List<EventSubImage>();
        foreach (var subImageFile in notification.SubImages)
        {
            var subImage =
                await _fileService.UploadAsync(subImageFile, $"{FileContainer.EVENTS}/{notification.EventId}");
            eventSubImages.Add(new EventSubImage
            {
                EventId = notification.EventId,
                ImageUrl = subImage.Blob.Uri ?? "",
                ImageFileName = subImage.Blob.Name ?? ""
            });
        }

        await _unitOfWork.EventSubImages.CreateListAsync(eventSubImages);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: UploadAndSaveEventSubImagesDomainEventHandler");
    }
}
using EventHub.Domain.Abstractions;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEmailContentDomainEventHandler : IDomainEventHandler<DeleteEmailContentDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly ILogger<DeleteEmailContentDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmailContentDomainEventHandler(IUnitOfWork unitOfWork, IFileService fileService,
        ILogger<DeleteEmailContentDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task Handle(DeleteEmailContentDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteEmailContentDomainEventHandler");

        var emailContents = _unitOfWork.EmailContents
            .FindByCondition(x => x.EventId.Equals(notification.EventId));

        var attachments = _unitOfWork.EmailAttachments
            .FindAll()
            .Join(
                emailContents,
                _attachment => _attachment.EmailContentId,
                _emailContent => _emailContent.Id,
                (_attachment, _emailContent) => _attachment);

        foreach (var attachment in attachments)
        {
            await _fileService.DeleteAsync($"{FileContainer.EVENTS}/{notification.EventId}",
                attachment.AttachmentFileName);
        }

        await attachments.ExecuteDeleteAsync();

        await emailContents.ExecuteDeleteAsync();

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: DeleteEmailContentDomainEventHandler");
    }
}
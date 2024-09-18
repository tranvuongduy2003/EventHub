using EventHub.Domain.Abstractions;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class CreateEmailContentOfEventDomainEventHandler : IDomainEventHandler<CreateEmailContentOfEventDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly ILogger<CreateEmailContentOfEventDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEmailContentOfEventDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<CreateEmailContentOfEventDomainEventHandler> logger, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _fileService = fileService;
    }

    public async Task Handle(CreateEmailContentOfEventDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateEmailContentOfEventDomainEventHandler");

        var emailContent = new EmailContent()
        {
            Content = notification.EmailContent.Content,
            EventId = notification.EventId
        };
        await _unitOfWork.EmailContents.CreateAsync(emailContent);
        await _unitOfWork.CommitAsync();

        if (notification.EmailContent.Attachments != null && notification.EmailContent.Attachments.Any())
        {
            var emailAttachments = new List<EmailAttachment>();
            foreach (var attachment in notification.EmailContent.Attachments)
            {
                var attachmentFile =
                    await _fileService.UploadAsync(attachment, $"{FileContainer.EVENTS}/{notification.EventId}");
                var emailAttachment = new EmailAttachment()
                {
                    AttachmentUrl = attachmentFile.Blob.Uri ?? "",
                    AttachmentFileName = attachmentFile.Blob.Name ?? "",
                    EmailContentId = emailContent.Id,
                };
                emailAttachments.Add(emailAttachment);
            }

            await _unitOfWork.EmailAttachments.CreateListAsync(emailAttachments);
            await _unitOfWork.CommitAsync();
        }

        _logger.LogInformation("END: CreateEmailContentOfEventDomainEventHandler");
    }
}
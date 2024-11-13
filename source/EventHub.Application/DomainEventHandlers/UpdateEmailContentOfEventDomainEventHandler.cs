using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class UpdateEmailContentOfEventDomainEventHandler : IDomainEventHandler<UpdateEmailContentOfEventDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateEmailContentOfEventDomainEventHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task Handle(UpdateEmailContentOfEventDomainEvent notification, CancellationToken cancellationToken)
    {
        var emailContent = await _unitOfWork.EmailContents.GetByIdAsync(notification.EmailContent.Id);
        if (emailContent == null)
            throw new NotFoundException("EmailContent does not exist!");

        emailContent.Content = notification.EmailContent.Content;

        var attachments = await _unitOfWork.EmailAttachments
            .FindByCondition(x => x.EmailContentId.Equals(emailContent.Id))
            .ToListAsync();
        foreach (var attachment in attachments)
        {
            await _fileService.DeleteAsync($"{FileContainer.EVENTS}/{notification.EventId}",
                attachment.AttachmentFileName);
        }

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
        }

        await _unitOfWork.EmailContents.UpdateAsync(emailContent);
        await _unitOfWork.CommitAsync();
    }
}
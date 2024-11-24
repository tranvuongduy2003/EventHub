using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Abstractions.Services;
using EventHub.Domain.AggregateModels.EventAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class DeleteEmailContentDomainEventHandler : IDomainEventHandler<DeleteEmailContentDomainEvent>
{
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteEmailContentDomainEventHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task Handle(DeleteEmailContentDomainEvent notification, CancellationToken cancellationToken)
    {
        IQueryable<EmailContent> emailContents = _unitOfWork.EmailContents
            .FindByCondition(x => x.EventId.Equals(notification.EventId));

        var attachments = _unitOfWork.EmailAttachments
            .FindAll()
            .AsEnumerable()
            .Join(
                emailContents.AsEnumerable(),
                _attachment => _attachment.EmailContentId,
                _emailContent => _emailContent.Id,
                (_attachment, _emailContent) => _attachment)
            .ToList();

        foreach (EmailAttachment attachment in attachments)
        {
            await _fileService.DeleteAsync($"{FileContainer.EVENTS}/{notification.EventId}",
                attachment.AttachmentFileName);
        }

        _unitOfWork.EmailAttachments.DeleteList(attachments);

        await emailContents.ExecuteDeleteAsync(cancellationToken);

        await _unitOfWork.CommitAsync();
    }
}

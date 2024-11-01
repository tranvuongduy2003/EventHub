﻿using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Abstractions.Services;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
    }
}
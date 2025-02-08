using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Notification.DeleteNotification;

public class DeleteNotificationCommandHandler : ICommandHandler<DeleteNotificationCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteNotificationCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.NotificationAggregate.Notification notification = await _unitOfWork.Notifications.GetByIdAsync(request.Id);
        if (notification is null)
        {
            throw new NotFoundException("Notification does not exist!");
        }

        await _unitOfWork.Notifications.Delete(notification);
        await _unitOfWork.CommitAsync();
    }
}

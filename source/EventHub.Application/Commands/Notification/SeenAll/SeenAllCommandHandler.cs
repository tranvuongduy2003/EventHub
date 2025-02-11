using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Notification.SeenAll;

public class SeenAllCommandHandler : ICommandHandler<SeenAllCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public SeenAllCommandHandler(IUnitOfWork unitOfWork, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }

    public async Task Handle(SeenAllCommand request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        IQueryable<Domain.Aggregates.NotificationAggregate.Notification> notificationsQuery = _unitOfWork.Notifications
            .FindByCondition(x => x.TargetUserId == Guid.Parse(userId) && !x.IsSeen);

        if (request.Type != null)
        {
            notificationsQuery = notificationsQuery.Where(x => x.Type == request.Type);
        }

        List<Domain.Aggregates.NotificationAggregate.Notification> notifications = await notificationsQuery.ToListAsync(cancellationToken);

        foreach (Domain.Aggregates.NotificationAggregate.Notification notification in notifications)
        {
            notification.IsSeen = true;
            await _unitOfWork.Notifications.Update(notification);
            await _unitOfWork.CommitAsync();
        }
    }
}

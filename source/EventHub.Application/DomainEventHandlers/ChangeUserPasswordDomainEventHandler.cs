using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.DomainEventHandlers;

public class ChangeUserPasswordDomainEventHandler : IDomainEventHandler<ChangeUserPasswordDomainEvent>
{
    private readonly UserManager<User> _userManager;

    public ChangeUserPasswordDomainEventHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(ChangeUserPasswordDomainEvent notification, CancellationToken cancellationToken)
    {
        User user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User does not exist!");
        }

        IdentityResult result =
            await _userManager.ChangePasswordAsync(user, notification.OldPassword, notification.NewPassword);

        if (!result.Succeeded)
        {
            throw new BadRequestException(result);
        }
    }
}

using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class ChangeUserPasswordDomainEventHandler : IDomainEventHandler<ChangeUserPasswordDomainEvent>
{
    private readonly ILogger<ChangeUserPasswordDomainEventHandler> _logger;
    private readonly UserManager<User> _userManager;

    public ChangeUserPasswordDomainEventHandler(ILogger<ChangeUserPasswordDomainEventHandler> logger,
        UserManager<User> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    public async Task Handle(ChangeUserPasswordDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: ChangeUserPasswordDomainEventHandler");

        var user = await _userManager.FindByIdAsync(notification.UserId.ToString());
        if (user == null)
            throw new NotFoundException("User does not exist!");

        var result = await _userManager.ChangePasswordAsync(user, notification.OldPassword, notification.NewPassword);

        if (!result.Succeeded)
            throw new BadRequestException(result);

        _logger.LogInformation("END: ChangeUserPasswordDomainEventHandler");
    }
}
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.User.UpdateUserRoles;

public class UpdateUserRolesCommandHandler : ICommandHandler<UpdateUserRolesCommand>
{
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public UpdateUserRolesCommandHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User does not exist!");
        }

        await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
        await _userManager.AddToRolesAsync(user, request.Roles);
    }
}

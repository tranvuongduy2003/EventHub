using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Event.DeleteEvent;

public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public DeleteEventCommandHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        await _unitOfWork.CachedEvents.SoftDelete(@event);

        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.NumberOfCreatedEvents--;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}

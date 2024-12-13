using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Event.RestoreEvent;

public class RestoreEventCommandHandler : ICommandHandler<RestoreEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public RestoreEventCommandHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task Handle(RestoreEventCommand request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)
            ?.Value ?? "";

        IQueryable<Domain.Aggregates.EventAggregate.Event> events = _unitOfWork.Events
            .GetDeletedEvents()
            .Join(
                request.Events,
                _event => _event.Id,
                _id => _id,
                (_event, _id) => _event);

        await _unitOfWork.CachedEvents.RestoreAsync(events, cancellationToken);

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.NumberOfCreatedEvents += events.Count();
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}

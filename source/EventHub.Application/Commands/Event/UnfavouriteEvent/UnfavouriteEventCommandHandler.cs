using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Event.UnfavouriteEvent;

public class UnfavouriteEventCommandHandler : ICommandHandler<UnfavouriteEventCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public UnfavouriteEventCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task Handle(UnfavouriteEventCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            var userId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");

            Domain.Aggregates.EventAggregate.Event
                .UnfavouriteEvent(userId, request.EventId);
        }, cancellationToken);
    }
}

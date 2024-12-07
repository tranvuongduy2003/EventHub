using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Event.FavouriteEvent;

public class FavouriteEventCommandHandler : ICommandHandler<FavouriteEventCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public FavouriteEventCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task Handle(FavouriteEventCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            var userId = Guid.Parse(_signInManager.Context.User.Claims
                .FirstOrDefault(x => x.Equals(JwtRegisteredClaimNames.Jti))?.Value ?? "");

            Domain.Aggregates.EventAggregate.Event
                .FavouriteEvent(userId, request.EventId);
        }, cancellationToken);
    }
}

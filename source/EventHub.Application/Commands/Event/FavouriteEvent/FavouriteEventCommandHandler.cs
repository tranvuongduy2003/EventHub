using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Event.FavouriteEvent;

public class FavouriteEventCommandHandler : ICommandHandler<FavouriteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public FavouriteEventCommandHandler(
        IUnitOfWork unitOfWork,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task Handle(FavouriteEventCommand request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Domain.Aggregates.UserAggregate.User user = (await _userManager.FindByIdAsync(userId))!;

        Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event is null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        bool isFavouriteEventExisted = await _unitOfWork.FavouriteEvents
            .ExistAsync(x =>
                x.EventId == request.EventId &&
                x.UserId == Guid.Parse(userId));

        if (isFavouriteEventExisted)
        {
            return;
        }

        await _unitOfWork.FavouriteEvents.CreateAsync(new Domain.Aggregates.EventAggregate.ValueObjects.FavouriteEvent
        {
            UserId = Guid.Parse(userId),
            EventId = request.EventId,
        });

        @event.NumberOfFavourites = (@event.NumberOfFavourites ?? 0) + 1;
        await _unitOfWork.CachedEvents.Update(@event);

        user.NumberOfFavourites = (user.NumberOfFavourites ?? 0) + 1;
        await _userManager.UpdateAsync(user);

        await _unitOfWork.CommitAsync();

        user.NumberOfFavourites++;
        await _userManager.UpdateAsync(user);
    }
}

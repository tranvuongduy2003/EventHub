﻿using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Commands.Event.UnfavouriteEvent;

public class UnfavouriteEventCommandHandler : ICommandHandler<UnfavouriteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public UnfavouriteEventCommandHandler(
        IUnitOfWork unitOfWork,
        SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task Handle(UnfavouriteEventCommand request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()
            ?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event is null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        @event.UnfavouriteEvent(Guid.Parse(userId), request.EventId);

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);
        if (user != null)
        {
            user.NumberOfFavourites--;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}

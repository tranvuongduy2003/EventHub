﻿using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.FavouriteEvent;

public class FavouriteEventCommandHandler : ICommandHandler<FavouriteEventCommand>
{
    public FavouriteEventCommandHandler()
    {
    }

    public async Task Handle(FavouriteEventCommand request, CancellationToken cancellationToken)
    {
        await Domain.AggregateModels.EventAggregate.Event
            .FavouriteEvent(request.UserId, request.EventId);
    }
}
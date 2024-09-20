using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.FavouriteEvent;

public class FavouriteEventCommandHandler : ICommandHandler<FavouriteEventCommand>
{
    private readonly ILogger<FavouriteEventCommandHandler> _logger;

    public FavouriteEventCommandHandler(ILogger<FavouriteEventCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(FavouriteEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: FavouriteEventCommandHandler");

        await Domain.AggregateModels.EventAggregate.Event
            .FavouriteEvent(request.UserId, request.EventId);

        _logger.LogInformation("END: FavouriteEventCommandHandler");
    }
}
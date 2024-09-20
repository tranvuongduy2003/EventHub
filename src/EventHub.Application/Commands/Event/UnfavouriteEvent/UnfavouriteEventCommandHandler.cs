using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.UnfavouriteEvent;

public class UnfavouriteEventCommandHandler : ICommandHandler<UnfavouriteEventCommand>
{
    private readonly ILogger<UnfavouriteEventCommandHandler> _logger;

    public UnfavouriteEventCommandHandler(ILogger<UnfavouriteEventCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(UnfavouriteEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UnfavouriteEventCommandHandler");

        await Domain.AggregateModels.EventAggregate.Event
            .UnfavouriteEvent(request.UserId, request.EventId);

        _logger.LogInformation("END: UnfavouriteEventCommandHandler");
    }
}
using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.RestoreEvent;

public class RestoreEventCommandHandler : ICommandHandler<RestoreEventCommand>
{
    private readonly ILogger<RestoreEventCommandHandler> _logger;

    public RestoreEventCommandHandler(ILogger<RestoreEventCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(RestoreEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: RestoreEventCommandHandler");

        await Domain.AggregateModels.EventAggregate.Event.RestoreEvent(request.UserId, request.Events);

        _logger.LogInformation("END: RestoreEventCommandHandler");
    }
}
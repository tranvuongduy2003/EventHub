using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.MakeEventsPublic;

public class MakeEventsPublicCommandHandler : ICommandHandler<MakeEventsPublicCommand>
{
    private readonly ILogger<MakeEventsPublicCommandHandler> _logger;

    public MakeEventsPublicCommandHandler(ILogger<MakeEventsPublicCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(MakeEventsPublicCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: MakeEventsPublicCommandHandler");

        await Domain.AggregateModels.EventAggregate.Event.MakeEventsPublic(request.UserId, request.Events);

        _logger.LogInformation("END: MakeEventsPublicCommandHandler");
    }
}
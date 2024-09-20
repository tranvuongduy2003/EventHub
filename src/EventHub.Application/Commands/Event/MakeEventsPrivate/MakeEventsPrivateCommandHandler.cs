using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.MakeEventsPrivate;

public class MakeEventsPrivateCommandHandler : ICommandHandler<MakeEventsPrivateCommand>
{
    private readonly ILogger<MakeEventsPrivateCommandHandler> _logger;

    public MakeEventsPrivateCommandHandler(ILogger<MakeEventsPrivateCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(MakeEventsPrivateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: MakeEventsPublicCommandHandler");

        await Domain.AggregateModels.EventAggregate.Event.MakeEventsPrivate(request.UserId, request.Events);

        _logger.LogInformation("END: MakeEventsPublicCommandHandler");
    }
}
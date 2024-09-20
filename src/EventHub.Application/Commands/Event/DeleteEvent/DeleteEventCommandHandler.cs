using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Event.DeleteEvent;

public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand>
{
    private readonly ILogger<DeleteEventCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public DeleteEventCommandHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        ILogger<DeleteEventCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteEventCommandHandler");

        var @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event == null)
            throw new NotFoundException("Event does not exist!");

        await _unitOfWork.Events.SoftDeleteAsync(@event);

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user != null)
        {
            user.NumberOfCreatedEvents--;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: DeleteEventCommandHandler");
    }
}
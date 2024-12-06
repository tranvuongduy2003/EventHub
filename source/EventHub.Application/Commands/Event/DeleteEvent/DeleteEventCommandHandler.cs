using EventHub.Application.Abstractions;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Event.DeleteEvent;

public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public DeleteEventCommandHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        await _unitOfWork.Events.SoftDelete(@event);

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user != null)
        {
            user.NumberOfCreatedEvents--;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}

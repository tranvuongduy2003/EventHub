using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Event.DeleteEvent;

public class DeleteEventCommandHandler : ICommandHandler<DeleteEventCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public DeleteEventCommandHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        Domain.AggregateModels.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        _unitOfWork.Events.SoftDelete(@event);

        Domain.AggregateModels.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user != null)
        {
            user.NumberOfCreatedEvents--;
            await _userManager.UpdateAsync(user);
        }

        await _unitOfWork.CommitAsync();
    }
}

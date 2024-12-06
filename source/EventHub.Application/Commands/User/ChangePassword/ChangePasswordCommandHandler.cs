using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.User.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    public ChangePasswordCommandHandler()
    {
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        await Task.Run(() =>
        {
            UserAggregateRoot.ChangeUserPassword(request.UserId, request.OldPassword, request.NewPassword);
        }, cancellationToken);
    }
}

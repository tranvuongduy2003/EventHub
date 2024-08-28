using EventHub.Application.Commands.User.CreateUser;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.SeedWork.Command;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.User.ChangePassword;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand>
{
    private readonly ILogger<CreateUserCommandHandler> _logger;

    public ChangePasswordCommandHandler(ILogger<CreateUserCommandHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: ChangePasswordCommandHandler");

        await UserAggregateRoot.ChangeUserPassword(request.UserId, request.OldPassword, request.NewPassword);

        _logger.LogInformation("END: ChangePasswordCommandHandler");
    }
}
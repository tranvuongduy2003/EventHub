using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.SignOut;

public class SignOutCommandHandler : ICommandHandler<SignOutCommand>
{
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public SignOutCommandHandler(SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
    }
}
using EventHub.Domain.AggregateModels.UserAggregate;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.SignOut;

public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
{
    private readonly SignInManager<User> _signInManager;

    public SignOutCommandHandler(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }
    
    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
    }
}
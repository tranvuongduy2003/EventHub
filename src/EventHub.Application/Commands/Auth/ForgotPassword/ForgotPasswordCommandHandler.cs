using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Services;
using EventHub.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;

    public ForgotPasswordCommandHandler(UserManager<User> userManager, IHangfireService hangfireService, IEmailService emailService)
    {
        _userManager = userManager;
        _hangfireService = hangfireService;
        _emailService = emailService;
    }
    
    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new NotFoundException("User does not exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetPasswordUrl = $"https://localhost:5173/reset-password?token={token}&email={request.Email}";

        _hangfireService.Enqueue(() =>
            _emailService
                .SendResetPasswordEmailAsync(request.Email, resetPasswordUrl)
                .Wait());

        return true;
    }
}
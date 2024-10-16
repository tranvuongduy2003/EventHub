using EventHub.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly IHangfireService _hangfireService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public ForgotPasswordCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        IHangfireService hangfireService, IEmailService emailService)
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
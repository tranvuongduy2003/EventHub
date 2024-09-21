using EventHub.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : ICommandHandler<ForgotPasswordCommand, bool>
{
    private readonly IEmailService _emailService;
    private readonly IHangfireService _hangfireService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public ForgotPasswordCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        IHangfireService hangfireService, IEmailService emailService, ILogger<ForgotPasswordCommandHandler> logger)
    {
        _userManager = userManager;
        _hangfireService = hangfireService;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: ForgotPasswordCommandHandler");

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null) throw new NotFoundException("User does not exist");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetPasswordUrl = $"https://localhost:5173/reset-password?token={token}&email={request.Email}";

        _hangfireService.Enqueue(() =>
            _emailService
                .SendResetPasswordEmailAsync(request.Email, resetPasswordUrl)
                .Wait());
        _logger.LogInformation("END: ForgotPasswordCommandHandler");

        return true;
    }
}
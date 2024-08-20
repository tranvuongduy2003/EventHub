using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Services;
using EventHub.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.ForgotPassword;

public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IHangfireService _hangfireService;
    private readonly IEmailService _emailService;
    private readonly ILogger<ForgotPasswordCommandHandler> _logger;

    public ForgotPasswordCommandHandler(UserManager<User> userManager, IHangfireService hangfireService, IEmailService emailService, ILogger<ForgotPasswordCommandHandler> logger)
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
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Auth.ValidateUser;

public class ValidateUserCommandHandler : ICommandHandler<ValidateUserCommand, bool>
{
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly ILogger<ValidateUserCommandHandler> _logger;

    public ValidateUserCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ILogger<ValidateUserCommandHandler> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<bool> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: ValidateUserCommandHandler");
        
        var useByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (useByEmail != null)
            throw new BadRequestException("Email already exists");

        var useByPhoneNumber = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (useByPhoneNumber != null)
            throw new BadRequestException("Phone number already exists");
        
        _logger.LogInformation("END: ValidateUserCommandHandler");

        return true;
    }
}
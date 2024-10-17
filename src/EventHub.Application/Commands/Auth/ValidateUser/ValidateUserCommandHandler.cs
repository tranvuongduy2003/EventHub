using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Auth.ValidateUser;

public class ValidateUserCommandHandler : ICommandHandler<ValidateUserCommand, bool>
{
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public ValidateUserCommandHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> Handle(ValidateUserCommand request, CancellationToken cancellationToken)
    {
        var useByEmail = await _userManager.FindByEmailAsync(request.Email);
        if (useByEmail != null)
            throw new BadRequestException("Email already exists");

        var useByPhoneNumber = await _userManager.Users
            .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (useByPhoneNumber != null)
            throw new BadRequestException("Phone number already exists");

        return true;
    }
}
using EventHub.Shared.DTOs.User;
using MediatR;

namespace EventHub.Application.Commands.Auth.ValidateUser;

public class ValidateUserCommand : IRequest<bool>
{
    public ValidateUserCommand(ValidateUserDto dto)
        => (Email, PhoneNumber, FullName) = (dto.Email, dto.PhoneNumber, dto.FullName);
    
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string FullName { get; set; }
}
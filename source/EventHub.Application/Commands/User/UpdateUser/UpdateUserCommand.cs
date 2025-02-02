using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.Shared.Enums.User;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.User.UpdateUser;

public class UpdateUserCommand : ICommand<UserDto>
{
    public UpdateUserCommand(Guid userId, UpdateUserDto request)
        => (UserId, Email, PhoneNumber, Dob, FullName, UserName, Gender, Bio, Avatar) = (userId, request.Email,
            request.PhoneNumber, request.Dob, request.FullName, request.UserName,
            request.Gender, request.Bio, request.Avatar);

    public Guid UserId { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public string FullName { get; set; }

    public string? UserName { get; set; }

    public EGender? Gender { get; set; }

    public string? Bio { get; set; }

    public IFormFile? Avatar { get; set; }
}

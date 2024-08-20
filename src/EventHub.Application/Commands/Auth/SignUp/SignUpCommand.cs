using EventHub.Shared.DTOs.Auth;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Enums.User;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Auth.SignUp;

public class SignUpCommand : IRequest<SignInResponseDto>
{
    public SignUpCommand(CreateUserDto dto)
        => (Email, PhoneNumber, Dob, FullName, Password, UserName, Gender, Bio, Avatar) 
            = (dto.Email, dto.PhoneNumber, dto.Dob, dto.FullName, dto.Password, dto.UserName, dto.Gender, dto.Bio, dto.Avatar);

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

    public string FullName { get; set; }

    public string Password { get; set; }

    public string? UserName { get; set; } = string.Empty;

    public EGender? Gender { get; set; }

    public string? Bio { get; set; }

    public IFormFile? Avatar { get; set; }
}
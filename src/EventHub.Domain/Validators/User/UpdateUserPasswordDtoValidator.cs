﻿using EventHub.Domain.DTOs.User;
using FluentValidation;

namespace EventHub.Domain.Validators.User;

public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDto>
{
    public UpdateUserPasswordDtoValidator()
    {
        RuleFor(x => x.OldPassword).NotEmpty().WithMessage("Old password is required");

        RuleFor(x => x.NewPassword).NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("Password has to at least 8 characters")
            .Matches(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$")
            .WithMessage("Password is not match complexity rules.");
        ;
    }
}
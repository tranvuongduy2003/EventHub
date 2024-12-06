using FluentValidation;

namespace EventHub.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(x => x.Id.ToString())
            .NotEmpty().WithMessage("Category ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MaximumLength(100).WithMessage("Category name cannot exceed 100 characters");

        RuleFor(x => x.IconImage)
            .NotNull().WithMessage("Icon image is required");

        RuleFor(x => x.Color)
            .NotEmpty().WithMessage("Color is required")
            .Matches(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$")
            .WithMessage("Color must be a valid hex color code (e.g. #FF0000)");
    }
}

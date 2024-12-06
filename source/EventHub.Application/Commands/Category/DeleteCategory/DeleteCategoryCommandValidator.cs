using FluentValidation;

namespace EventHub.Application.Commands.Category.DeleteCategory;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Id.ToString())
            .NotEmpty().WithMessage("Category ID is required");
    }
}

using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;

namespace EventHub.Application.Commands.Category.DeleteCategory;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Entities.Category category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category is null)
        {
            throw new NotFoundException("Category does not exist!");
        }

        bool isEventCategoryExisted = await _unitOfWork.EventCategories.ExistAsync(x => x.CategoryId == request.Id);
        if (isEventCategoryExisted)
        {
            throw new BadRequestException($"Existing more than 1 events in category {category.Name}");
        }

        if (!string.IsNullOrEmpty(category.IconImageFileName))
        {
            await _fileService.DeleteAsync(category.IconImageFileName, FileContainer.CATEGORIES);
        }

        await _unitOfWork.Categories.SoftDelete(category);
        await _unitOfWork.CommitAsync();
    }
}

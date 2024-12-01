using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.File;
using EventHub.Shared.ValueObjects;

namespace EventHub.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Domain.AggregateModels.CategoryAggregate.Category category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category is null)
        {
            throw new NotFoundException("Category does not exist!");
        }

        category.Color = request.Category.Color;
        category.Name = request.Category.Name;

        if (!string.IsNullOrEmpty(category.IconImageFileName))
        {
            await _fileService.DeleteAsync(category.IconImageFileName, FileContainer.CATEGORIES);
        }
        BlobResponseDto iconImage = await _fileService.UploadAsync(request.Category.IconImage, FileContainer.CATEGORIES);
        category.IconImageUrl = iconImage.Blob.Uri ?? "";
        category.IconImageFileName = iconImage.Blob.Name ?? "";

        await _unitOfWork.CachedCategories.Update(category);
        await _unitOfWork.CommitAsync();
    }
}

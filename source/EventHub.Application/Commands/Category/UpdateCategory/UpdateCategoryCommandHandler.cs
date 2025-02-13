using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.File;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;

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
        Domain.Aggregates.EventAggregate.Entities.Category category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category is null)
        {
            throw new NotFoundException("Category does not exist!");
        }

        bool isCategoryExisted = await _unitOfWork.Categories.ExistAsync(x => x.Name == request.Name && x.Id != request.Id);
        if (isCategoryExisted)
        {
            throw new BadRequestException("Category name already exists!");
        }

        category.Color = request.Color;
        category.Name = request.Name;

        if (!string.IsNullOrEmpty(category.IconImageFileName))
        {
            await _fileService.DeleteAsync(category.IconImageFileName, FileContainer.CATEGORIES);
        }
        BlobResponseDto iconImage = await _fileService.UploadAsync(request.IconImage, FileContainer.CATEGORIES);
        category.IconImageUrl = iconImage.Blob.Uri ?? "";
        category.IconImageFileName = iconImage.Blob.Name ?? "";

        await _unitOfWork.Categories.Update(category);
        await _unitOfWork.CommitAsync();
    }
}

using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Category.UpdateCategory;

public class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IFileService fileService,
        ILogger<UpdateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UpdateCategoryCommandHandler");

        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category is null)
            throw new NotFoundException("Category does not exist!");

        category.Color = request.Category.Color;
        category.Name = request.Category.Name;
        
        if (!string.IsNullOrEmpty(category.IconImageFileName))
            await _fileService.DeleteAsync(category.IconImageFileName, FileContainer.CATEGORIES);
        var iconImage = await _fileService.UploadAsync(request.Category.IconImage, FileContainer.CATEGORIES);
        category.IconImageUrl = iconImage.Blob.Uri ?? "";
        category.IconImageFileName = iconImage.Blob.Name ?? "";

        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: UpdateCategoryCommandHandler");
    }
}
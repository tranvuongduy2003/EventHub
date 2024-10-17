using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Category.DeleteCategory;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IFileService _fileService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IFileService fileService,
        ILogger<DeleteCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category is null)
            throw new NotFoundException("Category does not exist!");

        var isEventCategoryExisted = await _unitOfWork.EventCategories.ExistAsync(request.Id);

        if (isEventCategoryExisted)
            throw new BadRequestException($"Existing more than 1 events in category {category.Name}");

        if (!string.IsNullOrEmpty(category.IconImageFileName))
            await _fileService.DeleteAsync(category.IconImageFileName, FileContainer.CATEGORIES);

        await _unitOfWork.Categories.SoftDeleteAsync(category);
        await _unitOfWork.CommitAsync();
    }
}
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using EventHub.Shared.ValueObjects;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Category.DeleteCategory;

public class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileService _fileService;
    private readonly ILogger<DeleteCategoryCommandHandler> _logger;

    public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IFileService fileService, ILogger<DeleteCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _fileService = fileService;
        _logger = logger;
    }

    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteCategoryCommandHandler");
        
        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category is null)
            throw new NotFoundException("Category does not exist!");

        var isEventCategoryExisted = await _unitOfWork.EventCategories.ExistAsync(request.Id);

        if (isEventCategoryExisted)
            throw new BadRequestException($"Existing more than 1 events in category {category.Name}");

        //TODO: Delete icon image from FileStorage
        if (category.IconImageFileName != null)
            await _fileService.DeleteAsync(category.IconImageFileName, FileContainer.CATEGORIES);

        await _unitOfWork.Categories.SoftDeleteAsync(category);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("END: DeleteCategoryCommandHandler");
    }
}
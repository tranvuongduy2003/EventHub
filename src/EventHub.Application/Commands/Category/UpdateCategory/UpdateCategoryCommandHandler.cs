using AutoMapper;
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
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ILogger<UpdateCategoryCommandHandler> _logger;

    public UpdateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<UpdateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _logger = logger;
    }
    
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UpdateCategoryCommandHandler");

        var category = await _unitOfWork.Categories.GetByIdAsync(request.Id);
        if (category is null)
            throw new NotFoundException("Category does not exist!");
        
        //TODO: Delete current icon image from FileStorage
        if (category.IconImageFileName != null)
            await _fileService.DeleteAsync(category.IconImageFileName, FileContainer.CATEGORIES);

        category = _mapper.Map<Domain.AggregateModels.CategoryAggregate.Category>(request.Category);
        
        //TODO: Upload file and assign category.ImageColor to new File's Name
        var iconImage = await _fileService.UploadAsync(request.Category.IconImage, FileContainer.CATEGORIES);
        category.IconImageUrl = iconImage.Blob.Uri ?? "";
        category.IconImageFileName = iconImage.Blob.Name ?? "";
        
        await _unitOfWork.Categories.UpdateAsync(category);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("END: UpdateCategoryCommandHandler");
    }
}
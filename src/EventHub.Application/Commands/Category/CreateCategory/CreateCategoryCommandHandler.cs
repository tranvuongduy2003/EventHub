using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Category.CreateCategory;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IFileService _fileService;
    private readonly ILogger<CreateCategoryCommandHandler> _logger;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<CreateCategoryCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
        _logger = logger;
    }
    
    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateCategoryCommandHandler");

        var category = _mapper.Map<Domain.AggregateModels.CategoryAggregate.Category>(request);
        
        //TODO: Upload file and assign category.ImageColor to new File's Name
        var iconImage = await _fileService.UploadAsync(request.IconImage, FileContainer.CATEGORIES);
        category.IconImage = iconImage.Blob.Name;

        await _unitOfWork.Categories.CreateAsync(category);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("END: CreateCategoryCommandHandler");
        
        return _mapper.Map<CategoryDto>(category);
    }
}
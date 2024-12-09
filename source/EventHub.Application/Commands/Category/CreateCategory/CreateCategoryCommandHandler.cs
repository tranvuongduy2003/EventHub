using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Category;
using EventHub.Application.DTOs.File;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.Shared.Constants;

namespace EventHub.Application.Commands.Category.CreateCategory;

public class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _fileService = fileService;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Domain.Aggregates.CategoryAggregate.Category
        {
            Color = request.Color,
            Name = request.Name,
            IconImageUrl = string.Empty,
            IconImageFileName = string.Empty
        };

        BlobResponseDto iconImage = await _fileService.UploadAsync(request.IconImage, FileContainer.CATEGORIES);
        category.IconImageUrl = iconImage.Blob.Uri ?? "";
        category.IconImageFileName = iconImage.Blob.Name ?? "";

        await _unitOfWork.CachedCategories.CreateAsync(category);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CategoryDto>(category);
    }
}

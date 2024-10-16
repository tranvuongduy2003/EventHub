using AutoMapper;
using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.ValueObjects;

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
        var category = new Domain.AggregateModels.CategoryAggregate.Category
        {
            Color = request.Color,
            Name = request.Name,
            IconImageUrl = string.Empty,
            IconImageFileName = string.Empty
        };

        var iconImage = await _fileService.UploadAsync(request.IconImage, FileContainer.CATEGORIES);
        category.IconImageUrl = iconImage.Blob.Uri ?? "";
        category.IconImageFileName = iconImage.Blob.Name ?? "";

        await _unitOfWork.Categories.CreateAsync(category);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<CategoryDto>(category);
    }
}
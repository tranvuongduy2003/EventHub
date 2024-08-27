using AutoMapper;
using EventHub.Application.Queries.Category.GetPaginatedCategories;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Category.GetCategoryById;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPaginatedCategoriesQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetPaginatedCategoriesQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetCategoryByIdQueryHandler");

        var cachedCategory = await _unitOfWork.CachedCategories.GetByIdAsync(request.Id);

        if (cachedCategory == null)
            throw new NotFoundException("Category does not exist!");

        var category = _mapper.Map<CategoryDto>(cachedCategory);

        _logger.LogInformation("END: GetCategoryByIdQueryHandler");

        return category;
    }
}
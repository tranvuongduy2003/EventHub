using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Category.GetCategoryById;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{
    private readonly ILogger<GetCategoryByIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetCategoryByIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetCategoryByIdQueryHandler");

        var cachedCategory = await _unitOfWork.CachedCategories.GetByIdAsync(request.CategoryId);

        if (cachedCategory == null)
            throw new NotFoundException("Category does not exist!");

        var category = _mapper.Map<CategoryDto>(cachedCategory);

        _logger.LogInformation("END: GetCategoryByIdQueryHandler");

        return category;
    }
}
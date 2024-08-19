using AutoMapper;
using EventHub.Application.Queries.Category.GetPaginatedCategories;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Models.Category;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Category.GetCategoryById;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryModel>
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

    public async Task<CategoryModel> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetCategoryByIdQueryHandler");

        var cachedCategory = await _unitOfWork.CachedCategories.GetByIdAsync(request.Id);

        var category = _mapper.Map<CategoryModel>(cachedCategory);

        _logger.LogInformation("END: GetCategoryByIdQueryHandler");

        return category;
    }
}
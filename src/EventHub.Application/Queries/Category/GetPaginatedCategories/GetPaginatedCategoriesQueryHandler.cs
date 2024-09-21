using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Category.GetPaginatedCategories;

public class GetPaginatedCategoriesQueryHandler : IQueryHandler<GetPaginatedCategoriesQuery, Pagination<CategoryDto>>
{
    private readonly ILogger<GetPaginatedCategoriesQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedCategoriesQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetPaginatedCategoriesQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<CategoryDto>> Handle(GetPaginatedCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPaginatedCategoriesQueryHandler");

        var cachedCategories = _unitOfWork.CachedCategories.FindAll();

        var categories = _mapper.Map<List<CategoryDto>>(cachedCategories);

        _logger.LogInformation("END: GetPaginatedCategoriesQueryHandler");

        return PagingHelper.Paginate<CategoryDto>(categories, request.Filter);
    }
}
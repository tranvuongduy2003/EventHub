using AutoMapper;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Helpers;
using EventHub.Shared.Models.Category;
using EventHub.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Category.GetPaginatedCategories;

public class GetPaginatedCategoriesQueryHandler : IRequestHandler<GetPaginatedCategoriesQuery, Pagination<CategoryModel>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetPaginatedCategoriesQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetPaginatedCategoriesQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetPaginatedCategoriesQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<CategoryModel>> Handle(GetPaginatedCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPaginatedCategoriesQueryHandler");

        var cachedCategories = await _unitOfWork.CachedCategories.FindAll();
        
        var categories = _mapper.Map<List<CategoryModel>>(cachedCategories);

        _logger.LogInformation("END: GetPaginatedCategoriesQueryHandler");

        return PagingHelper.Paginate<CategoryModel>(categories, request.Filter);
    }
}
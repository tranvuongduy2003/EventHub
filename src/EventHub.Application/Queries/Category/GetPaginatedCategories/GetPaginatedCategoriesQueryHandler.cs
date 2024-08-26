using AutoMapper;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Category.GetPaginatedCategories;

public class GetPaginatedCategoriesQueryHandler : IRequestHandler<GetPaginatedCategoriesQuery, Pagination<CategoryDto>>
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
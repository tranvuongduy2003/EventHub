using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Category;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Category.GetPaginatedCategories;

public class GetPaginatedCategoriesQueryHandler : IQueryHandler<GetPaginatedCategoriesQuery, Pagination<CategoryDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<CategoryDto>> Handle(GetPaginatedCategoriesQuery request,
        CancellationToken cancellationToken)
    {

        List<Domain.Aggregates.CategoryAggregate.Category> cachedCategories = await _unitOfWork.CachedCategories
            .FindAll()
            .ToListAsync(cancellationToken);

        List<CategoryDto> categories = _mapper.Map<List<CategoryDto>>(cachedCategories);

        return PagingHelper.Paginate<CategoryDto>(categories, request.Filter);
    }
}

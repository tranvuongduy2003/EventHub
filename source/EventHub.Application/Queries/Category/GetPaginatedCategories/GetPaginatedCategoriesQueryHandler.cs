using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
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

        List<Domain.AggregateModels.CategoryAggregate.Category> cachedCategories = await _unitOfWork.CachedCategories
            .FindAll()
            .ToListAsync(cancellationToken);

        List<CategoryDto> categories = _mapper.Map<List<CategoryDto>>(cachedCategories);

        return PagingHelper.Paginate<CategoryDto>(categories, request.Filter);
    }
}
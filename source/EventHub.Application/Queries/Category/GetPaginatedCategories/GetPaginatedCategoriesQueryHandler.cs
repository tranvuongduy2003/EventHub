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

    public Task<Pagination<CategoryDto>> Handle(GetPaginatedCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.CategoryAggregate.Category> paginatedCategories =
            _unitOfWork.CachedCategories.PaginatedFind(
                request.Filter,
                query => query
                    .Include(x => x.EventCategories)
                    .ThenInclude(x => x.Category)
            );

        Pagination<CategoryDto> paginatedCategoryDtos = _mapper.Map<Pagination<CategoryDto>>(paginatedCategories);

        return Task.FromResult(paginatedCategoryDtos);
    }
}

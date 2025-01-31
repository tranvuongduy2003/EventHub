using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Category;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;

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
        Pagination<Domain.Aggregates.EventAggregate.Entities.Category> paginatedCategories = _unitOfWork.CachedCategories.PaginatedFind(request.Filter);

        Pagination<CategoryDto> paginatedCategoryDtos = _mapper.Map<Pagination<CategoryDto>>(paginatedCategories);

        return Task.FromResult(paginatedCategoryDtos);
    }
}

using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Category;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Category.GetCategoryById;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Entities.Category cachedCategory = await _unitOfWork.CachedCategories.GetByIdAsync(request.CategoryId);

        if (cachedCategory == null)
        {
            throw new NotFoundException("Category does not exist!");
        }

        CategoryDto category = _mapper.Map<CategoryDto>(cachedCategory);

        return category;
    }
}

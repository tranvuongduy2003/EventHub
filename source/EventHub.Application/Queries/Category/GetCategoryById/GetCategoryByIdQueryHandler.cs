using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Category;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Category.GetCategoryById;

public class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDto>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {


        var cachedCategory = await _unitOfWork.CachedCategories.GetByIdAsync(request.CategoryId);

        if (cachedCategory == null)
            throw new NotFoundException("Category does not exist!");

        var category = _mapper.Map<CategoryDto>(cachedCategory);

        return category;
    }
}
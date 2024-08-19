using EventHub.Shared.Models.Category;
using EventHub.Shared.SeedWork;
using MediatR;

namespace EventHub.Application.Queries.Category.GetPaginatedCategories;

public record GetPaginatedCategoriesQuery(PaginationFilter Filter) : IRequest<Pagination<CategoryModel>>;
using EventHub.Shared.DTOs.Category;
using MediatR;

namespace EventHub.Application.Queries.Category.GetCategoryById;

public record GetCategoryByIdQuery(string Id) : IRequest<CategoryDto>;
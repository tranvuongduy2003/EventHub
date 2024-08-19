using EventHub.Shared.Models.Category;
using MediatR;

namespace EventHub.Application.Queries.Category.GetCategoryById;

public record GetCategoryByIdQuery(string Id) : IRequest<CategoryModel>;
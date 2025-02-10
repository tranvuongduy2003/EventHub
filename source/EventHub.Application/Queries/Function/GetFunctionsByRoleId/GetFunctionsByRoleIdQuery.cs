using EventHub.Application.SeedWork.DTOs.Function;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Function.GetFunctionsByRoleId;

public record GetFunctionsByRoleIdQuery(Guid RoleId) : IQuery<List<FunctionDto>>;

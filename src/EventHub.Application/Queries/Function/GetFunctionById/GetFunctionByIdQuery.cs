using EventHub.Shared.DTOs.Function;
using MediatR;

namespace EventHub.Application.Queries.Function.GetFunctionById;

public record GetFunctionByIdQuery(string Id) : IRequest<FunctionDto>;
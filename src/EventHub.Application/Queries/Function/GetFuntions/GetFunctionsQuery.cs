using EventHub.Shared.DTOs.Function;
using MediatR;

namespace EventHub.Application.Queries.Function.GetFuntions;

public class GetFunctionsQuery : IRequest<List<FunctionDto>>;
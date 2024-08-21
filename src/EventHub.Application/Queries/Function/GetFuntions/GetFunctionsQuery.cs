using EventHub.Shared.DTOs.Function;
using MediatR;

namespace EventHub.Application.Queries.Function.GetFuntions;

/// <summary>
/// Represents a query to retrieve a list of all functions.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of all available functions without any specific filtering or pagination.
/// </remarks>
public class GetFunctionsQuery : IRequest<List<FunctionDto>>;
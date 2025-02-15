using EventHub.Application.SeedWork.DTOs.Function;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Function.GetFunctions;

/// <summary>
/// Represents a query to retrieve a list of all functions.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of all available functions without any specific filtering or pagination.
/// </remarks>
public class GetFunctionsQuery : IQuery<List<FunctionDto>>;

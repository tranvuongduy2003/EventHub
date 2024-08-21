using EventHub.Shared.DTOs.Function;
using MediatR;

namespace EventHub.Application.Queries.Function.GetFunctionById;

/// <summary>
/// Represents a query to retrieve a function's details by its unique identifier.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of a function's information based on its unique identifier.
/// </remarks>
public record GetFunctionByIdQuery(string Id) : IRequest<FunctionDto>;
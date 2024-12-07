using EventHub.Application.SeedWork.DTOs.Command;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Command.GetCommandsInFunction;

/// <summary>
/// Represents a query to retrieve a list of commands associated with a specific function.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of commands that are linked to a given function, specified by its unique identifier.
/// </remarks>
public record GetCommandsInFunctionQuery(string FunctionId) : IQuery<List<CommandDto>>;
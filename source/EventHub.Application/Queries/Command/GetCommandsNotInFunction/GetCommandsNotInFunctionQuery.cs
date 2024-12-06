using EventHub.Application.DTOs.Command;
using EventHub.Domain.SeedWork.Query;

namespace EventHub.Application.Queries.Command.GetCommandsNotInFunction;

/// <summary>
/// Represents a query to retrieve a list of commands that are not associated with a specific function.
/// </summary>
/// <remarks>
/// This query is used to request the retrieval of commands that are not linked to a given function, specified by its unique identifier.
/// </remarks>
public record GetCommandsNotInFunctionQuery(string FunctionId) : IQuery<List<CommandDto>>;
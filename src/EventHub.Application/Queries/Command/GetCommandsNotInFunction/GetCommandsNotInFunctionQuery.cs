using EventHub.Shared.DTOs.Command;
using MediatR;

namespace EventHub.Application.Queries.Command.GetCommandsNotInFunction;

public record GetCommandsNotInFunctionQuery(string FunctionId) : IRequest<List<CommandDto>>;
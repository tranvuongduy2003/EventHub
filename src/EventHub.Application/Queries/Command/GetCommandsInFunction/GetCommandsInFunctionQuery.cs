using EventHub.Shared.DTOs.Command;
using MediatR;

namespace EventHub.Application.Queries.Command.GetCommandsInFunction;

public record GetCommandsInFunctionQuery(string FunctionId) : IRequest<List<CommandDto>>;
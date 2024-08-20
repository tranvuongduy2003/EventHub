using MediatR;

namespace EventHub.Application.Commands.Function.DeleteFunction;

public record DeleteFunctionCommand(string FunctionId) : IRequest;
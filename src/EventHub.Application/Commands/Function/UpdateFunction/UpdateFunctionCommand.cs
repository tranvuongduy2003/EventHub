using EventHub.Shared.DTOs.Function;
using MediatR;

namespace EventHub.Application.Commands.Function.UpdateFunction;

public class UpdateFunctionCommand : IRequest
{
    public UpdateFunctionCommand(string id, UpdateFunctionDto request)
        => (Id, Function) = (id, request);

    public string Id { get; set; }

    public UpdateFunctionDto Function { get; set; }
}
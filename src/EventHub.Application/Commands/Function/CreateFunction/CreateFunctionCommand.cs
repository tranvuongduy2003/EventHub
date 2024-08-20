using EventHub.Shared.DTOs.Function;
using MediatR;

namespace EventHub.Application.Commands.Function.CreateFunction;

public class CreateFunctionCommand : IRequest<FunctionDto>
{
    public CreateFunctionCommand(CreateFunctionDto request)
        => (Name, Url, SortOrder, ParentId) =
            (request.Name, request.Url, request.SortOrder, request.ParentId);

    public string Name { get; set; }

    public string Url { get; set; }

    public int SortOrder { get; set; }

    public string? ParentId { get; set; }
}
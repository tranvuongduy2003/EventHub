using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Command;
using Microsoft.AspNetCore.Http;

namespace EventHub.Application.Commands.Event.CreateEvent;

public class CreateEmailContentCommand : ICommand<CreateEmailContentDto>
{
    public CreateEmailContentCommand()
    {
    }

    public CreateEmailContentCommand(CreateEmailContentDto request)
    {
        Content = request.Content;
        Attachments = request.Attachments;
    }

    public string Content { get; set; }

    public IFormFileCollection? Attachments { get; set; }
}

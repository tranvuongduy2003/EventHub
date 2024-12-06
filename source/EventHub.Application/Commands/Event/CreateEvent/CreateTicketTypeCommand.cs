using EventHub.Application.DTOs.Event;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.CreateEvent;


public class CreateTicketTypeCommand : ICommand<CreateTicketTypeDto>
{
    public CreateTicketTypeCommand()
    {
    }

    public CreateTicketTypeCommand(CreateTicketTypeDto request)
    {
        Name = request.Name;
        Quantity = request.Quantity;
        Price = request.Price;
    }

    public string Name { get; set; } = string.Empty;


    public int Quantity { get; set; }

    public long Price { get; set; }
}

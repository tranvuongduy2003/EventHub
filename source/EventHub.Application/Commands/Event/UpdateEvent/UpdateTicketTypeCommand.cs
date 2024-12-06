using EventHub.Application.DTOs.Event;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Event.UpdateEvent;


public class UpdateTicketTypeCommand : ICommand<UpdateTicketTypeDto>
{
    public UpdateTicketTypeCommand()
    {
    }

    public UpdateTicketTypeCommand(UpdateTicketTypeDto request)
    {
        Id = request.Id;
        Name = request.Name;
        Quantity = request.Quantity;
        Price = request.Price;
    }

    public Guid? Id { get; set; }

    public string Name { get; set; } = string.Empty;


    public int Quantity { get; set; }

    public long Price { get; set; }
}

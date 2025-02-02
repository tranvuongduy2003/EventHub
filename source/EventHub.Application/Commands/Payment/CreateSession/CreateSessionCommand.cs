using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Payment.CreateSession;

public class CreateSessionCommand : ICommand<CreateSessionResponseDto>
{
    public CreateSessionCommand(CreateSessionDto request)
    {
        ApprovedUrl = request.ApprovedUrl;
        CancelUrl = request.CancelUrl;
        PaymentId = request.PaymentId;
    }

    public string ApprovedUrl { get; set; }

    public string CancelUrl { get; set; }

    public Guid PaymentId { get; set; }
}

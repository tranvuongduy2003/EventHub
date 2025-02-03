using EventHub.Application.Commands.Payment.Checkout;
using EventHub.Application.Commands.Payment.ValidateSession;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly ILogger<PaymentsController> _logger;
    private readonly IMediator _mediator;

    public PaymentsController(ILogger<PaymentsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> PostCheckout([FromBody] CheckoutDto request)
    {
        _logger.LogInformation("START: PostCheckout");

        CheckoutResponseDto response = await _mediator.Send(new CheckoutCommand(request));

        _logger.LogInformation("END: PostCheckout");

        return Ok(new ApiOkResponse(response));
    }

    [HttpPost("{paymentId}/validate-session")]
    public async Task<IActionResult> PostValidateSession(Guid paymentId)
    {
        _logger.LogInformation("START: PostValidateSession");

        ValidateSessionResponseDto response = await _mediator.Send(new ValidateSessionCommand(paymentId));

        _logger.LogInformation("END: PostValidateSession");

        return Ok(new ApiOkResponse(response));
    }
}

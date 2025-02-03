using EventHub.Application.Commands.Payment.Checkout;
using EventHub.Application.Commands.Payment.ValidateSession;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerResponse(200, "Successfully checkout", typeof(CheckoutResponseDto))]
    public async Task<IActionResult> PostCheckout([FromBody] CheckoutDto request)
    {
        _logger.LogInformation("START: PostCheckout");

        CheckoutResponseDto response = await _mediator.Send(new CheckoutCommand(request));

        _logger.LogInformation("END: PostCheckout");

        return Ok(new ApiOkResponse(response));
    }

    [HttpPost("{paymentId}/validate-session")]
    [SwaggerResponse(200, "Successfully validate session", typeof(ValidateSessionResponseDto))]
    public async Task<IActionResult> PostValidateSession(Guid paymentId)
    {
        _logger.LogInformation("START: PostValidateSession");

        ValidateSessionResponseDto response = await _mediator.Send(new ValidateSessionCommand(paymentId));

        _logger.LogInformation("END: PostValidateSession");

        return Ok(new ApiOkResponse(response));
    }
}

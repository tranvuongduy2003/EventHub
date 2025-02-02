using EventHub.Application.Commands.Payment.Checkout;
using EventHub.Application.Commands.Payment.CreateSession;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/payments")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(ILogger<PaymentsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost("checkout")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.CREATE)]
    public async Task<IActionResult> PostCheckout(CheckoutDto request)
    {
        _logger.LogInformation("START: PostCheckout");

        await _mediator.Send(new CheckoutCommand(request));

        _logger.LogInformation("END: PostCheckout");

        return Ok(new ApiOkResponse());
    }

    [HttpPost("create-session")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.CREATE)]
    public async Task<IActionResult> PostCreateSession(CreateSessionDto request)
    {
        _logger.LogInformation("START: PostCreateSession");

        await _mediator.Send(new CreateSessionCommand(request));

        _logger.LogInformation("END: PostCreateSession");

        return Ok(new ApiOkResponse());
    }
}

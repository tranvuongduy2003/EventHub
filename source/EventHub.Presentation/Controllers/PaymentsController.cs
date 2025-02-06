using EventHub.Application.Commands.Payment.Checkout;
using EventHub.Application.Commands.Payment.ValidateSession;
using EventHub.Application.Queries.Payment.GetCreatedEventsPaymentStatistics;
using EventHub.Application.Queries.Payment.GetEventPaymentStatistics;
using EventHub.Application.Queries.Payment.GetPaginatedPayments;
using EventHub.Application.Queries.Payment.GetPaginatedPaymentsByCreatedEvents;
using EventHub.Application.Queries.Payment.GetPaginatedPaymentsByEventId;
using EventHub.Application.Queries.Payment.GetPaginatedPaymentsByUserId;
using EventHub.Application.Queries.Payment.GetPaymentById;
using EventHub.Application.Queries.Payment.GetUserPaymentStatistics;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
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

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of payments",
        Description = "Fetches a paginated list of payments based on the provided filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of payments", typeof(Pagination<PaymentDto>))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedPayments([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedPayments");

        Pagination<PaymentDto> payments = await _mediator.Send(new GetPaginatedPaymentsQuery(filter));

        _logger.LogInformation("END: GetPaginatedPayments");

        return Ok(new ApiOkResponse(payments));
    }

    [HttpGet("get-by-event/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of payments by the event",
        Description =
            "Fetches a paginated list of payments created by the event, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of payments", typeof(Pagination<PaymentDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedPaymentsByEvent(Guid eventId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedPaymentsByEvent");
        try
        {
            Pagination<PaymentDto> payments = await _mediator.Send(new GetPaginatedPaymentsByEventIdQuery(eventId, filter));

            _logger.LogInformation("END: GetPaginatedPaymentsByEvent");

            return Ok(new ApiOkResponse(payments));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("get-by-created-events/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of payments by the created events",
        Description =
            "Fetches a paginated list of payments created by the created events, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of payments", typeof(Pagination<PaymentDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedPaymentsByCreatedEvents(Guid userId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedPaymentsByCreatedEvents");
        try
        {
            Pagination<PaymentDto> payments = await _mediator.Send(new GetPaginatedPaymentsByCreatedEventsQuery(userId, filter));

            _logger.LogInformation("END: GetPaginatedPaymentsByCreatedEvents");

            return Ok(new ApiOkResponse(payments));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("get-by-user/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of payments by the user",
        Description =
            "Fetches a paginated list of payments created by the user, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of payments", typeof(Pagination<PaymentDto>))]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedPaymentsByUser(Guid userId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedPaymentsByUser");
        try
        {
            Pagination<PaymentDto> payments = await _mediator.Send(new GetPaginatedPaymentsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetPaginatedPaymentsByUser");

            return Ok(new ApiOkResponse(payments));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("get-by-event/{eventId:guid}/statistics")]
    [SwaggerResponse(200, "Successfully retrieved the statistics of payments", typeof(PaymentStatisticsDto))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetEventPaymentStatistics(Guid eventId)
    {
        _logger.LogInformation("START: GetEventPaymentStatistics");

        PaymentStatisticsDto statistics = await _mediator.Send(new GetEventPaymentStatisticsQuery(eventId));

        _logger.LogInformation("END: GetEventPaymentStatistics");

        return Ok(new ApiOkResponse(statistics));
    }

    [HttpGet("get-by-created-events/{userId:guid}/statistics")]
    [SwaggerResponse(200, "Successfully retrieved the statistics of payments", typeof(PaymentStatisticsDto))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCreatedEventsPaymentStatistics(Guid userId)
    {
        _logger.LogInformation("START: GetCreatedEventsPaymentStatistics");

        PaymentStatisticsDto statistics = await _mediator.Send(new GetCreatedEventsPaymentStatisticsQuery(userId));

        _logger.LogInformation("END: GetCreatedEventsPaymentStatistics");

        return Ok(new ApiOkResponse(statistics));
    }

    [HttpGet("get-by-user/{userId:guid}/statistics")]
    [SwaggerResponse(200, "Successfully retrieved the statistics of payments", typeof(PaymentStatisticsDto))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetUserPaymentStatistics(Guid userId)
    {
        _logger.LogInformation("START: GetUserPaymentStatistics");

        PaymentStatisticsDto statistics = await _mediator.Send(new GetUserPaymentStatisticsQuery(userId));

        _logger.LogInformation("END: GetUserPaymentStatistics");

        return Ok(new ApiOkResponse(statistics));
    }

    [HttpGet("{paymentId:guid}")]
    [SwaggerOperation(
    Summary = "Retrieve a payment by its ID",
    Description = "Fetches the details of a specific payment based on the provided payment ID."
)]
    [SwaggerResponse(200, "Successfully retrieved the payment", typeof(bool))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Payment with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaymentById(Guid paymentId)
    {
        _logger.LogInformation("START: GetPaymentById");
        try
        {
            PaymentDto payment = await _mediator.Send(new GetPaymentByIdQuery(paymentId));

            _logger.LogInformation("END: GetPaymentById");

            return Ok(new ApiOkResponse(payment));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPost("checkout")]
    [SwaggerResponse(200, "Successfully checkout", typeof(CheckoutResponseDto))]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.CREATE)]
    public async Task<IActionResult> PostCheckout([FromBody] CheckoutDto request)
    {
        try
        {
            _logger.LogInformation("START: PostCheckout");

            CheckoutResponseDto response = await _mediator.Send(new CheckoutCommand(request));

            _logger.LogInformation("END: PostCheckout");

            return Ok(new ApiOkResponse(response));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiBadRequestResponse(ex.Message));
        }
    }

    [HttpPost("{paymentId}/validate-session")]
    [SwaggerResponse(200, "Successfully validate session", typeof(ValidateSessionResponseDto))]
    [ClaimRequirement(EFunctionCode.GENERAL_PAYMENT, ECommandCode.UPDATE)]
    public async Task<IActionResult> PostValidateSession(Guid paymentId)
    {
        try
        {
            _logger.LogInformation("START: PostValidateSession");

            ValidateSessionResponseDto response = await _mediator.Send(new ValidateSessionCommand(paymentId));

            _logger.LogInformation("END: PostValidateSession");

            return Ok(new ApiOkResponse(response));
        }
        catch (BadRequestException ex)
        {
            return BadRequest(new ApiBadRequestResponse(ex.Message));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
    }
}

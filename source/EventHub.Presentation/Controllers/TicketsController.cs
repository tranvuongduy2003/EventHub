using EventHub.Application.Commands.Ticket.ActivateTicket;
using EventHub.Application.Commands.Ticket.CancelTicket;
using EventHub.Application.Commands.Ticket.CheckIn;
using EventHub.Application.Queries.Ticket.GetPaginatedTickets;
using EventHub.Application.Queries.Ticket.GetPaginatedTicketsByEventId;
using EventHub.Application.Queries.Ticket.GetPaginatedTicketsByUserId;
using EventHub.Application.Queries.Ticket.GetTicketById;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Ticket;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/tickets")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ILogger<TicketsController> _logger;
    private readonly IMediator _mediator;

    public TicketsController(ILogger<TicketsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of tickets",
        Description = "Fetches a paginated list of tickets based on the provided filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of tickets", typeof(Pagination<TicketDto>))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_TICKET, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedTickets([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedTickets");

        Pagination<TicketDto> tickets = await _mediator.Send(new GetPaginatedTicketsQuery(filter));

        _logger.LogInformation("END: GetPaginatedTickets");

        return Ok(new ApiOkResponse(tickets));
    }

    [HttpGet("get-by-event/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of tickets by the event",
        Description =
            "Fetches a paginated list of tickets created by the event, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of tickets", typeof(Pagination<TicketDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_TICKET, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedTicketsByEvent(Guid eventId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedTicketsByEvent");
        try
        {
            Pagination<TicketDto> tickets = await _mediator.Send(new GetPaginatedTicketsByEventIdQuery(eventId, filter));

            _logger.LogInformation("END: GetPaginatedTicketsByEvent");

            return Ok(new ApiOkResponse(tickets));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("get-by-user/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of tickets by the user",
        Description =
            "Fetches a paginated list of tickets created by the user, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of tickets", typeof(Pagination<TicketDto>))]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_TICKET, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedTicketsByUser(Guid userId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedTicketsByUser");
        try
        {
            Pagination<TicketDto> tickets = await _mediator.Send(new GetPaginatedTicketsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetPaginatedTicketsByUser");

            return Ok(new ApiOkResponse(tickets));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("{ticketId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a ticket by its ID",
        Description = "Fetches the details of a specific ticket based on the provided ticket ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the ticket", typeof(bool))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Ticket with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_TICKET, ECommandCode.VIEW)]
    public async Task<IActionResult> GetTicketById(Guid ticketId)
    {
        _logger.LogInformation("START: GetTicketById");
        try
        {
            TicketDto ticket = await _mediator.Send(new GetTicketByIdQuery(ticketId));

            _logger.LogInformation("END: GetTicketById");

            return Ok(new ApiOkResponse(ticket));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPatch("{ticketId:guid}/activate")]
    [SwaggerOperation(
        Summary = "Activate a ticket",
        Description = "Activate a ticket based on the provided details."
    )]
    [SwaggerResponse(200, "Ticket activated successfully", typeof(bool))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_TICKET, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchActivateTicket(Guid ticketId)
    {
        try
        {
            _logger.LogInformation("START: PostActivateTicket");

            await _mediator.Send(new ActivateTicketCommand(ticketId));

            _logger.LogInformation("END: PostActivateTicket");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
    }

    [HttpPatch("{ticketId:guid}/check-in")]
    [SwaggerOperation(
        Summary = "Check-in a ticket",
        Description = "Check-in a ticket based on the provided details."
    )]
    [SwaggerResponse(200, "Ticket check-ined successfully", typeof(bool))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_TICKET, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchCheckIn(Guid ticketId)
    {
        try
        {
            _logger.LogInformation("START: PostCheckIn");

            await _mediator.Send(new CheckInCommand(ticketId));

            _logger.LogInformation("END: PostCheckIn");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
    }

    [HttpPatch("{ticketId:guid}/cancel")]
    [SwaggerOperation(
        Summary = "Cancel a ticket",
        Description = "Cancel a ticket based on the provided details."
    )]
    [SwaggerResponse(200, "Ticket canceled successfully", typeof(bool))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_TICKET, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchCancelTicket(Guid ticketId)
    {
        try
        {
            _logger.LogInformation("START: PostCancelTicket");

            await _mediator.Send(new CancelTicketCommand(ticketId));

            _logger.LogInformation("END: PostCancelTicket");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
    }
}


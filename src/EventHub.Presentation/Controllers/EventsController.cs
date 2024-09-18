using EventHub.Application.Queries.Event.GetEventById;
using EventHub.Application.Queries.Event.GetPaginatedEvents;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Event;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using EventHub.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

public class EventsController : ControllerBase
{
    private readonly ILogger<EventsController> _logger;
    private readonly IMediator _mediator;

    public EventsController(ILogger<EventsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of events",
        Description = "Fetches a paginated list of events based on the provided filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of events", typeof(Pagination<EventDto>))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> GetPaginatedEvents([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedEvents");
        try
        {
            var events = await _mediator.Send(new GetPaginatedEventsQuery(filter));

            _logger.LogInformation("END: GetPaginatedEvents");

            return Ok(new ApiOkResponse(events));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a event by its ID",
        Description = "Fetches the details of a specific event based on the provided event ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the event", typeof(EventDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.VIEW)]
    public async Task<IActionResult> GetEventById(Guid eventId)
    {
        _logger.LogInformation("START: GetEventById");
        try
        {
            var @event = await _mediator.Send(new GetEventByIdQuery(eventId));

            _logger.LogInformation("END: GetEventById");

            return Ok(new ApiOkResponse(@event));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }
}
using EventHub.Application.Commands.Event.CreateEvent;
using EventHub.Application.Commands.Event.DeleteEvent;
using EventHub.Application.Commands.Event.FavouriteEvent;
using EventHub.Application.Commands.Event.MakeEventsPrivate;
using EventHub.Application.Commands.Event.MakeEventsPublic;
using EventHub.Application.Commands.Event.PermanentlyDeleteEvent;
using EventHub.Application.Commands.Event.RestoreEvent;
using EventHub.Application.Commands.Event.UnfavouriteEvent;
using EventHub.Application.Commands.Event.UpdateEvent;
using EventHub.Application.Queries.Event.GetCreatedEventsByUserId;
using EventHub.Application.Queries.Event.GetDeletedEventsByUserId;
using EventHub.Application.Queries.Event.GetEventById;
using EventHub.Application.Queries.Event.GetFavouriteEventsByUserId;
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

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new event",
        Description = "Creates a new event with the provided details."
    )]
    [SwaggerResponse(200, "Event created successfully", typeof(EventDto))]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.CREATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostCreateEvent([FromForm] CreateEventDto request)
    {
        _logger.LogInformation("START: PostCreateEvent");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            var events = await _mediator.Send(new CreateEventCommand(request, userId));

            _logger.LogInformation("END: PostCreateEvent");

            return Ok(new ApiOkResponse(events));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("get-created-events")]
    [SwaggerOperation(
        Summary = "Retrieve created events",
        Description = "Fetches a paginated list of events created by the user, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved events", typeof(Pagination<EventDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCreatedEvents([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetCreatedEvents");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            var events = await _mediator.Send(new GetCreatedEventsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetCreatedEvents");

            return Ok(new ApiOkResponse(events));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Update an existing event",
        Description = "Updates the details of an existing event based on the provided event ID and update information."
    )]
    [SwaggerResponse(200, "Event updated successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid input or request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PutUpdateEvent(Guid eventId, [FromForm] UpdateEventDto request)
    {
        _logger.LogInformation("START: PutUpdateEvent");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new UpdateEventCommand(eventId, request, userId));

            _logger.LogInformation("END: PutUpdateEvent");

            return Ok(new ApiOkResponse());
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Delete an existing event",
        Description = "Deletes an existing event based on the provided event ID."
    )]
    [SwaggerResponse(200, "Event deleted successfully")]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteEvent(Guid eventId)
    {
        _logger.LogInformation("START: DeleteEvent");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new DeleteEventCommand(userId, eventId));

            _logger.LogInformation("END: DeleteEvent");

            return Ok(new ApiOkResponse());
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

    [HttpDelete("delete-permanently/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Permanently delete an event",
        Description = "Permanently deletes an existing event based on the provided event ID."
    )]
    [SwaggerResponse(200, "Event permanently deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteEventPermanently(Guid eventId)
    {
        _logger.LogInformation("START: DeleteEventPermanently");
        try
        {
            await _mediator.Send(new PermanentlyDeleteEventCommand(eventId));

            _logger.LogInformation("END: DeleteEventPermanently");

            return Ok(new ApiOkResponse());
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

    [HttpPatch("restore")]
    [SwaggerOperation(
        Summary = "Restore deleted events",
        Description = "Restores a list of deleted events based on the provided event IDs."
    )]
    [SwaggerResponse(200, "Events restored successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchRestoreDeletedEvent([FromBody] List<Guid> events)
    {
        _logger.LogInformation("START: PatchRestoreDeletedEvent");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new RestoreEventCommand(userId, events));

            _logger.LogInformation("END: PatchRestoreDeletedEvent");

            return Ok(new ApiOkResponse());
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("get-deleted-events")]
    [SwaggerOperation(
        Summary = "Retrieve deleted events",
        Description =
            "Fetches a paginated list of events that have been deleted, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved deleted events", typeof(Pagination<EventDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetDeletedEvents([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetDeletedEvents");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            var events = await _mediator.Send(new GetDeletedEventsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetDeletedEvents");

            return Ok(new ApiOkResponse(events));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPatch("favourite/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Mark an event as favourite",
        Description = "Marks an existing event as a favourite based on the provided event ID."
    )]
    [SwaggerResponse(200, "Event marked as favourite successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchFavouriteEvent(Guid eventId)
    {
        _logger.LogInformation("START: PatchFavouriteEvent");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new FavouriteEventCommand(userId, eventId));

            _logger.LogInformation("END: PatchFavouriteEvent");

            return Ok(new ApiOkResponse());
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPatch("unfavourite/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Unmark an event as favourite",
        Description = "Removes an event from the user's favourites based on the provided event ID."
    )]
    [SwaggerResponse(200, "Event unfavourited successfully")]
    [SwaggerResponse(400, "BadRequest - Invalid request data")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> PatchUnfavouriteEvent(Guid eventId)
    {
        _logger.LogInformation("START: PatchUnfavouriteEvent");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new UnfavouriteEventCommand(userId, eventId));

            _logger.LogInformation("END: PatchUnfavouriteEvent");

            return Ok(new ApiOkResponse());
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
        catch (BadRequestException e)
        {
            return BadRequest(new ApiBadRequestResponse(e.Message));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("get-favourite-events")]
    [SwaggerOperation(
        Summary = "Retrieve favourite events",
        Description =
            "Fetches a paginated list of events marked as favourites by the user, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved favourite events", typeof(Pagination<EventDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetFavouriteEvents([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetFavouriteEvents");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            var events = await _mediator.Send(new GetFavouriteEventsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetFavouriteEvents");

            return Ok(new ApiOkResponse(events));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPatch("make-events-private")]
    [SwaggerOperation(
        Summary = "Make events private",
        Description = "Changes the visibility of specified events to private based on the provided event IDs."
    )]
    [SwaggerResponse(200, "Events marked as private successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchMakeEventsPrivate([FromBody] List<Guid> events)
    {
        _logger.LogInformation("START: PatchMakeEventsPrivate");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new MakeEventsPrivateCommand(userId, events));

            _logger.LogInformation("END: PatchMakeEventsPrivate");

            return Ok(new ApiOkResponse());
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPatch("make-events-public")]
    [SwaggerOperation(
        Summary = "Make events public",
        Description = "Changes the visibility of specified events to public based on the provided event IDs."
    )]
    [SwaggerResponse(200, "Events marked as public successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchMakeEventsPublic([FromBody] List<Guid> events)
    {
        _logger.LogInformation("START: PatchMakeEventsPublic");
        try
        {
            var userId = (Guid)HttpContext.Items["UserId"];

            await _mediator.Send(new MakeEventsPublicCommand(userId, events));

            _logger.LogInformation("END: PatchMakeEventsPublic");

            return Ok(new ApiOkResponse());
        }
        catch (Exception)
        {
            throw;
        }
    }
}
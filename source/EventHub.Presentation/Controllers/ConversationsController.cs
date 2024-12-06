using EventHub.Application.Attributes;
using EventHub.Application.DTOs.Conversation;
using EventHub.Application.DTOs.Message;
using EventHub.Application.Exceptions;
using EventHub.Application.Queries.Conversation.GetConversationsByEventId;
using EventHub.Application.Queries.Conversation.GetConversationsByUserId;
using EventHub.Application.Queries.Conversation.GetMessagesByConversationId;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/conversations")]
[ApiController]
public class ConversationsController : ControllerBase
{
    private readonly ILogger<ConversationsController> _logger;
    private readonly IMediator _mediator;

    public ConversationsController(ILogger<ConversationsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("get-by-event/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of conversations by the event",
        Description =
            "Fetches a paginated list of conversations created by the event, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of conversations", typeof(Pagination<ConversationDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_CHAT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetConversationsByEvent(Guid eventId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetConversationsByEvent");
        try
        {
            Pagination<ConversationDto> conversations = await _mediator.Send(new GetConversationsByEventIdQuery(eventId, filter));

            _logger.LogInformation("END: GetConversationsByEvent");

            return Ok(new ApiOkResponse(conversations));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("get-by-user/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of conversations by the user",
        Description =
            "Fetches a paginated list of conversations created by the user, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of conversations", typeof(Pagination<ConversationDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_CHAT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetConversationsByUser(Guid userId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetConversationsByUser");
        try
        {
            Pagination<ConversationDto> conversations = await _mediator.Send(new GetConversationsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetConversationsByUser");

            return Ok(new ApiOkResponse(conversations));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("{conversationId:guid}/messages")]
    [SwaggerOperation(
        Summary = "Retrieve a list of messages by the conversation",
        Description =
            "Fetches a paginated list of messages created by the conversation, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of messages", typeof(Pagination<MessageDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Conversation with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_CHAT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetMessagesByConversation(Guid conversationId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetMessagesByConversation");
        try
        {
            Pagination<MessageDto> conversations = await _mediator.Send(new GetMessagesByConversationIdQuery(conversationId, filter));

            _logger.LogInformation("END: GetMessagesByConversation");

            return Ok(new ApiOkResponse(conversations));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }
}

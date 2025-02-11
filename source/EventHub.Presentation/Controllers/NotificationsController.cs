using EventHub.Application.Commands.Notification.DeleteNotification;
using EventHub.Application.Commands.Notification.SeenAll;
using EventHub.Application.Queries.Notification.GetPaginatedNotificationsByUserId;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Notification;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.Enums.Notification;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/notifications")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly ILogger<NotificationsController> _logger;
    private readonly IMediator _mediator;

    public NotificationsController(ILogger<NotificationsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet()]
    [SwaggerOperation(
        Summary = "Retrieve a list of notifications by the user",
        Description =
            "Fetches a paginated list of notifications created by the user, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of notifications", typeof(Pagination<NotificationDto>))]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedNotifications([FromQuery] NotificationPaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedNotifications");
        try
        {
            Pagination<NotificationDto, NotificationMetadata> notifications = await _mediator.Send(new GetPaginatedNotificationsQuery(filter));

            _logger.LogInformation("END: GetPaginatedNotifications");

            return Ok(new ApiOkResponse(notifications));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPatch("seen-all")]
    [SwaggerResponse(200, "Seen updated successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Review with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EVENT, ECommandCode.UPDATE)]
    public async Task<IActionResult> PatchSeenAllNotifications([FromQuery] ENotificationType? type)
    {
        _logger.LogInformation("START: PatchSeenAllNotifications");

        await _mediator.Send(new SeenAllCommand(type));

        _logger.LogInformation("END: PatchSeenAllNotifications");

        return Ok(new ApiOkResponse(true));
    }

    [HttpDelete("{notificationId:guid}")]
    [SwaggerOperation(
        Summary = "Delete a notification",
        Description = "Deletes the notification with the specified ID."
    )]
    [SwaggerResponse(200, "Notification deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Notification with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_REVIEW, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteNotification(Guid notificationId)
    {
        _logger.LogInformation("START: DeleteNotification");
        try
        {
            await _mediator.Send(new DeleteNotificationCommand(notificationId));

            _logger.LogInformation("END: DeleteNotification");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }
}

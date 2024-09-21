using EventHub.Application.Commands.Review.CreateReview;
using EventHub.Application.Commands.Review.DeleteReview;
using EventHub.Application.Commands.Review.UpdateReview;
using EventHub.Application.Queries.Review.GetPaginatedReviews;
using EventHub.Application.Queries.Review.GetPaginatedReviewsByEventId;
using EventHub.Application.Queries.Review.GetPaginatedReviewsByUserId;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Review;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using EventHub.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/reviews")]
[ApiController]
public class ReviewsController : ControllerBase
{
    private readonly ILogger<ReviewsController> _logger;
    private readonly IMediator _mediator;

    public ReviewsController(ILogger<ReviewsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new review",
        Description = "Creates a new review based on the provided details."
    )]
    [SwaggerResponse(201, "Review created successfully", typeof(ReviewDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_REVIEW, ECommandCode.CREATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostCreateReview([FromBody] CreateReviewDto request)
    {
        _logger.LogInformation("START: PostCreateReview");
        try
        {
            var review = await _mediator.Send(new CreateReviewCommand(request));

            _logger.LogInformation("END: PostCreateReview");

            return Ok(new ApiCreatedResponse(review));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of reviews",
        Description = "Fetches a paginated list of reviews based on the provided filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of reviews", typeof(Pagination<ReviewDto>))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> GetPaginatedReviews([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedReviews");
        try
        {
            var reviews = await _mediator.Send(new GetPaginatedReviewsQuery(filter));

            _logger.LogInformation("END: GetPaginatedReviews");

            return Ok(new ApiOkResponse(reviews));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("get-by-event/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of reviews by the event",
        Description =
            "Fetches a paginated list of reviews created by the event, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of reviews", typeof(Pagination<ReviewDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> GetPaginatedReviewsByEvent(Guid eventId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedReviewsByEvent");
        try
        {
            var reviews = await _mediator.Send(new GetPaginatedReviewsByEventIdQuery(eventId, filter));

            _logger.LogInformation("END: GetPaginatedReviewsByEvent");

            return Ok(new ApiOkResponse(reviews));
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

    [HttpGet("get-by-user/{userId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of reviews by the user",
        Description =
            "Fetches a paginated list of reviews created by the user, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of reviews", typeof(Pagination<ReviewDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - User with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_REVIEW, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedReviewsByUser(Guid userId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedReviewsByUser");
        try
        {
            var reviews = await _mediator.Send(new GetPaginatedReviewsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetPaginatedReviewsByUser");

            return Ok(new ApiOkResponse(reviews));
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

    [HttpPut("{reviewId:guid}")]
    [SwaggerOperation(
        Summary = "Update an existing review",
        Description =
            "Updates the details of an existing review based on the provided review ID and update information."
    )]
    [SwaggerResponse(200, "Review updated successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Review with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_REVIEW, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PutUpdateReview(Guid reviewId, [FromBody] UpdateReviewDto request)
    {
        _logger.LogInformation("START: PutUpdateReview");
        try
        {
            await _mediator.Send(new UpdateReviewCommand(reviewId, request));

            _logger.LogInformation("END: PutUpdateReview");

            return Ok(new ApiOkResponse(true));
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

    [HttpDelete("{reviewId:guid}")]
    [SwaggerOperation(
        Summary = "Delete a review",
        Description = "Deletes the review with the specified ID."
    )]
    [SwaggerResponse(200, "Review deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Review with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_REVIEW, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteReview(Guid reviewId)
    {
        _logger.LogInformation("START: DeleteReview");
        try
        {
            await _mediator.Send(new DeleteReviewCommand(reviewId));

            _logger.LogInformation("END: DeleteReview");

            return Ok(new ApiOkResponse(true));
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
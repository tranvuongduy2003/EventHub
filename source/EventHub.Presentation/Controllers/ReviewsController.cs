﻿using EventHub.Application.Commands.Review.CreateReview;
using EventHub.Application.Commands.Review.DeleteReview;
using EventHub.Application.Commands.Review.UpdateReview;
using EventHub.Application.Queries.Review.GetPaginatedReviews;
using EventHub.Application.Queries.Review.GetPaginatedReviewsByCreatedEvents;
using EventHub.Application.Queries.Review.GetPaginatedReviewsByEventId;
using EventHub.Application.Queries.Review.GetPaginatedReviewsByUserId;
using EventHub.Application.Queries.Review.GetReviewById;
using EventHub.Application.Queries.Review.GetReviewStatisticsByCreatedEvents;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
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
    public async Task<IActionResult> PostCreateReview([FromBody] CreateReviewDto request)
    {
        _logger.LogInformation("START: PostCreateReview");

        ReviewDto review = await _mediator.Send(new CreateReviewCommand(request));

        _logger.LogInformation("END: PostCreateReview");

        return Ok(new ApiCreatedResponse(review));
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

        Pagination<ReviewDto> reviews = await _mediator.Send(new GetPaginatedReviewsQuery(filter));

        _logger.LogInformation("END: GetPaginatedReviews");

        return Ok(new ApiOkResponse(reviews));
    }

    [HttpGet("get-by-created-events")]
    [SwaggerOperation(
        Summary = "Retrieve a list of reviews by the created events",
        Description =
            "Fetches a paginated list of reviews created by the created events, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of reviews", typeof(Pagination<ReviewDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_REVIEW, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedReviewsByCreatedEvents([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedReviewsByCreatedEvents");
        try
        {
            Pagination<ReviewDto> reviews = await _mediator.Send(new GetPaginatedReviewsByCreatedEventsQuery(filter));

            _logger.LogInformation("END: GetPaginatedReviewsByCreatedEvents");

            return Ok(new ApiOkResponse(reviews));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("get-by-created-events/{userId:guid}/statistics")]
    [SwaggerResponse(200, "Successfully retrieved the list of reviews", typeof(Pagination<ReviewDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_REVIEW, ECommandCode.VIEW)]
    public async Task<IActionResult> GetReviewStatisticsByCreatedEvents(Guid userId)
    {
        _logger.LogInformation("START: GetReviewStatisticsByCreatedEvents");
        try
        {
            ReviewStatisticsDto statistics = await _mediator.Send(new GetReviewStatisticsByCreatedEventsQuery(userId));

            _logger.LogInformation("END: GetReviewStatisticsByCreatedEvents");

            return Ok(new ApiOkResponse(statistics));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
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
            Pagination<ReviewDto> reviews = await _mediator.Send(new GetPaginatedReviewsByEventIdQuery(eventId, filter));

            _logger.LogInformation("END: GetPaginatedReviewsByEvent");

            return Ok(new ApiOkResponse(reviews));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
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
            Pagination<ReviewDto> reviews = await _mediator.Send(new GetPaginatedReviewsByUserIdQuery(userId, filter));

            _logger.LogInformation("END: GetPaginatedReviewsByUser");

            return Ok(new ApiOkResponse(reviews));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("{reviewId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a review by its ID",
        Description = "Fetches the details of a specific review based on the provided review ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the review", typeof(ReviewDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Review with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.VIEW)]
    public async Task<IActionResult> GetReviewById(Guid reviewId)
    {
        _logger.LogInformation("START: GetReviewById");
        try
        {
            ReviewDto review = await _mediator.Send(new GetReviewByIdQuery(reviewId));

            _logger.LogInformation("END: GetReviewById");

            return Ok(new ApiOkResponse(review));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
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
    }
}

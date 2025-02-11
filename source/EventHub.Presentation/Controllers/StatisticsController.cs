using EventHub.Application.Queries.Event.GetEventReviewsByCustomer;
using EventHub.Application.Queries.Event.GetEventTotalStatistic;
using EventHub.Application.SeedWork.DTOs.Statistic;
using EventHub.Domain.Shared.HttpResponses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/statistics")]
[ApiController]
public class StatisticsController : ControllerBase
#pragma warning restore CA1515
{
    private readonly IMediator _mediator;
    private readonly ILogger<StatisticsController> _logger;

    public StatisticsController(ILogger<StatisticsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet("event-total-statistic/{eventId:guid}")]
    [SwaggerResponse(200, "Successfully retrieved the list of events", typeof(EventTotalStatisticDto))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> GetEventTotalStatistic(Guid eventId)
    {
        _logger.LogInformation("START: GetEventTotalStatistic");

        EventTotalStatisticDto statistic = await _mediator.Send(new GetEventTotalStatisticQuery(eventId));

        _logger.LogInformation("END: GetEventTotalStatistic");

        return Ok(new ApiOkResponse(statistic));
    }

    [HttpGet("event-reviews-by-customer/{eventId:guid}")]
    [SwaggerResponse(200, "Successfully retrieved the list of events", typeof(EventReviewsByCustomerDto))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> GetEventReviewsByCustomer(Guid eventId)
    {
        _logger.LogInformation("START: GetEventReviewsByCustomer");

        EventReviewsByCustomerDto statistic = await _mediator.Send(new GetEventReviewsByCustomerQuery(eventId));

        _logger.LogInformation("END: GetEventReviewsByCustomer");

        return Ok(new ApiOkResponse(statistic));
    }

}

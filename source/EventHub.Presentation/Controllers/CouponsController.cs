using EventHub.Application.Commands.Coupon.CreateCoupon;
using EventHub.Application.Commands.Coupon.DeleteCoupon;
using EventHub.Application.Commands.Coupon.UpdateCoupon;
using EventHub.Application.Queries.Coupon.GetCouponById;
using EventHub.Application.Queries.Coupon.GetCoupons;
using EventHub.Application.Queries.Coupon.GetCreatedCouponsByUserId;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Coupon;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/coupons")]
[ApiController]
public class CouponsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CouponsController> _logger;

    public CouponsController(ILogger<CouponsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new coupon",
        Description = "Creates a new coupon based on the provided details."
    )]
    [SwaggerResponse(201, "Coupon created successfully", typeof(CouponDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_COUPON, ECommandCode.CREATE)]
    public async Task<IActionResult> PostCreateCoupon([FromBody] CreateCouponDto request)
    {
        try
        {
            _logger.LogInformation("START: PostCreateCoupon");

            CouponDto coupon = await _mediator.Send(new CreateCouponCommand(request));

            _logger.LogInformation("END: PostCreateCoupon");

            return Ok(new ApiCreatedResponse(coupon));
        }
        catch (NotFoundException ex)
        {
            return NotFound(new ApiNotFoundResponse(ex.Message));
        }
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of coupons",
        Description = "Fetches a list of all available coupons."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of coupons", typeof(Pagination<CouponDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> GetCoupons([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetCoupons");

        Pagination<CouponDto> coupons = await _mediator.Send(new GetCouponsQuery(filter));

        _logger.LogInformation("END: GetCoupons");

        return Ok(new ApiOkResponse(coupons));
    }
    
    [HttpGet("get-created-coupons")]
    [SwaggerOperation(
        Summary = "Retrieve a list of created coupons",
        Description = "Fetches a list of all available created coupons."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of coupons", typeof(Pagination<CouponDto>))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_COUPON, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCreatedCoupons([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetCreatedCoupons");

        Pagination<CouponDto> coupons = await _mediator.Send(new GetCreatedCouponsByUserIdQuery(filter));

        _logger.LogInformation("END: GetCreatedCoupons");

        return Ok(new ApiOkResponse(coupons));
    }

    [HttpGet("{couponId}")]
    [SwaggerOperation(
        Summary = "Retrieve a coupon by its ID",
        Description = "Fetches the details of a specific coupon based on the provided coupon ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the coupon", typeof(CouponDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Coupon with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_COUPON, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCouponById(Guid couponId)
    {
        _logger.LogInformation("START: GetCouponById");
        try
        {
            CouponDto coupon = await _mediator.Send(new GetCouponByIdQuery(couponId));

            _logger.LogInformation("END: GetCouponById");

            return Ok(new ApiOkResponse(coupon));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPut("{couponId}")]
    [SwaggerOperation(
        Summary = "Update an existing coupon",
        Description = "Updates the details of an existing coupon based on the provided coupon ID and update information."
    )]
    [SwaggerResponse(200, "Coupon updated successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Coupon with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_COUPON, ECommandCode.UPDATE)]
    public async Task<IActionResult> PutUpdateCoupon(Guid couponId, [FromBody] UpdateCouponDto request)
    {
        _logger.LogInformation("START: PutUpdateCoupon");
        try
        {
            await _mediator.Send(new UpdateCouponCommand(couponId, request));

            _logger.LogInformation("END: PutUpdateCoupon");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpDelete("{couponId}")]
    [SwaggerOperation(
        Summary = "Delete a coupon",
        Description = "Deletes the coupon with the specified ID."
    )]
    [SwaggerResponse(200, "Coupon deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Coupon with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_COUPON, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteCoupon(Guid couponId)
    {
        _logger.LogInformation("START: DeleteCoupon");
        try
        {
            await _mediator.Send(new DeleteCouponCommand(couponId));

            _logger.LogInformation("END: DeleteCoupon");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }
}

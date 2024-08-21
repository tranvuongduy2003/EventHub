using AutoMapper;
using EventHub.Application.Commands.Category.CreateCategory;
using EventHub.Application.Commands.Category.DeleteCategory;
using EventHub.Application.Commands.Category.UpdateCategory;
using EventHub.Application.Queries.Category.GetCategoryById;
using EventHub.Application.Queries.Category.GetPaginatedCategories;
using EventHub.Infrastructure.FilterAttributes;
using EventHub.Shared.DTOs.Category;
using EventHub.Shared.Enums.Command;
using EventHub.Shared.Enums.Function;
using EventHub.Shared.Exceptions;
using EventHub.Shared.HttpResponses;
using EventHub.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/categories")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.CREATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostCategory([FromForm] CreateCategoryDto request)
    {
        _logger.LogInformation("START: PostCategory");
        try
        {
            var category = await _mediator.Send(new CreateCategoryCommand(request));

            _logger.LogInformation("END: PostCategory");

            return Ok(new ApiCreatedResponse(category));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(Pagination<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCategories([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetCategories");
        try
        {
            var categories = await _mediator.Send(new GetPaginatedCategoriesQuery(filter));

            _logger.LogInformation("END: GetCategories");

            return Ok(new ApiOkResponse(categories));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("{categoryId}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCategoryById(string categoryId)
    {
        _logger.LogInformation("START: GetCategoryById");
        try
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(categoryId));

            _logger.LogInformation("END: GetCategoryById");

            return Ok(new ApiOkResponse(category));
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

    [HttpPut("{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PutCategory(string categoryId, [FromForm] UpdateCategoryDto request)
    {
        _logger.LogInformation("START: PutCategory");
        try
        {
            await _mediator.Send(new UpdateCategoryCommand(categoryId, request));

            _logger.LogInformation("END: PutCategory");

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

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteCategory(string categoryId)
    {
        _logger.LogInformation("START: DeleteCategory");
        try
        {
            await _mediator.Send(new DeleteCategoryCommand(categoryId));

            _logger.LogInformation("END: DeleteCategory");

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
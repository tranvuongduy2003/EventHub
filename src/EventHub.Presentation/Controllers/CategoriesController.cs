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
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(
        Summary = "Create a new category",
        Description = "Creates a new category based on the provided details. The request must include multipart form data."
    )]
    [SwaggerResponse(201, "Category created successfully", typeof(CategoryDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.CREATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PostCreateCategory([FromForm] CreateCategoryDto request)
    {
        _logger.LogInformation("START: PostCreateCategory");
        try
        {
            var category = await _mediator.Send(new CreateCategoryCommand(request));

            _logger.LogInformation("END: PostCreateCategory");

            return Ok(new ApiCreatedResponse(category));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of categories",
        Description = "Fetches a paginated list of categories based on the provided filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of categories", typeof(Pagination<CategoryDto>))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    public async Task<IActionResult> GetPaginatedCategories([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedCategories");
        try
        {
            var categories = await _mediator.Send(new GetPaginatedCategoriesQuery(filter));

            _logger.LogInformation("END: GetPaginatedCategories");

            return Ok(new ApiOkResponse(categories));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("{categoryId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a category by its ID",
        Description = "Fetches the details of a specific category based on the provided category ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the category", typeof(CategoryDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Category with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.VIEW)]
    public async Task<IActionResult> GetCategoryById(Guid categoryId)
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

    [HttpPut("{categoryId:guid}")]
    [SwaggerOperation(
        Summary = "Update an existing category",
        Description = "Updates the details of an existing category based on the provided category ID and update information."
    )]
    [SwaggerResponse(200, "Category updated successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Category with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [Consumes("multipart/form-data")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.UPDATE)]
    [ApiValidationFilter]
    public async Task<IActionResult> PutUpdateCategory(Guid categoryId, [FromForm] UpdateCategoryDto request)
    {
        _logger.LogInformation("START: PutUpdateCategory");
        try
        {
            await _mediator.Send(new UpdateCategoryCommand(categoryId, request));

            _logger.LogInformation("END: PutUpdateCategory");

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

    [HttpDelete("{categoryId:guid}")]
    [SwaggerOperation(
        Summary = "Delete a category",
        Description = "Deletes the category with the specified ID."
    )]
    [SwaggerResponse(200, "Category deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Category with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_CATEGORY, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteCategory(Guid categoryId)
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
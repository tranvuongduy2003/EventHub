using EventHub.Application.Commands.Expense.CreateExpense;
using EventHub.Application.Commands.Expense.CreateSubExpense;
using EventHub.Application.Commands.Expense.DeleteExpense;
using EventHub.Application.Commands.Expense.DeleteSubExpense;
using EventHub.Application.Commands.Expense.UpdateExpense;
using EventHub.Application.Commands.Expense.UpdateSubExpense;
using EventHub.Application.Queries.Expense.GetExpenseById;
using EventHub.Application.Queries.Expense.GetPaginatedExpenses;
using EventHub.Application.Queries.Expense.GetPaginatedExpensesByEventId;
using EventHub.Application.Queries.Expense.GetSubExpensesByExpenseId;
using EventHub.Application.SeedWork.Attributes;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Shared.Enums.Command;
using EventHub.Domain.Shared.Enums.Function;
using EventHub.Domain.Shared.HttpResponses;
using EventHub.Domain.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventHub.Presentation.Controllers;

[Route("api/v1/expenses")]
[ApiController]
public class ExpensesController : ControllerBase
{
    private readonly ILogger<ExpensesController> _logger;
    private readonly IMediator _mediator;

    public ExpensesController(ILogger<ExpensesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation(
        Summary = "Create a new expense",
        Description = "Creates a new expense based on the provided details."
    )]
    [SwaggerResponse(201, "Expense created successfully", typeof(ExpenseDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.CREATE)]
    public async Task<IActionResult> PostCreateExpense([FromBody] CreateExpenseDto request)
    {
        _logger.LogInformation("START: PostCreateExpense");

        ExpenseDto expense = await _mediator.Send(new CreateExpenseCommand(request));

        _logger.LogInformation("END: PostCreateExpense");

        return Ok(new ApiCreatedResponse(expense));
    }

    [HttpGet]
    [SwaggerOperation(
        Summary = "Retrieve a list of expenses",
        Description = "Fetches a paginated list of expenses based on the provided filter parameters."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of expenses", typeof(Pagination<ExpenseDto>))]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedExpenses([FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedExpenses");

        Pagination<ExpenseDto> expenses = await _mediator.Send(new GetPaginatedExpensesQuery(filter));

        _logger.LogInformation("END: GetPaginatedExpenses");

        return Ok(new ApiOkResponse(expenses));
    }

    [HttpGet("get-by-event/{eventId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a list of expenses by the event",
        Description =
            "Fetches a paginated list of expenses created by the event, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of expenses", typeof(Pagination<ExpenseDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.VIEW)]
    public async Task<IActionResult> GetPaginatedExpensesByEvent(Guid eventId, [FromQuery] PaginationFilter filter)
    {
        _logger.LogInformation("START: GetPaginatedExpensesByEvent");
        try
        {
            Pagination<ExpenseDto> expenses = await _mediator.Send(new GetPaginatedExpensesByEventIdQuery(eventId, filter));

            _logger.LogInformation("END: GetPaginatedExpensesByEvent");

            return Ok(new ApiOkResponse(expenses));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpGet("{expenseId:guid}")]
    [SwaggerOperation(
        Summary = "Retrieve a expense by its ID",
        Description = "Fetches the details of a specific expense based on the provided expense ID."
    )]
    [SwaggerResponse(200, "Successfully retrieved the expense", typeof(ExpenseDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Expense with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.VIEW)]
    public async Task<IActionResult> GetExpenseById(Guid expenseId)
    {
        _logger.LogInformation("START: GetExpenseById");
        try
        {
            ExpenseDto expense = await _mediator.Send(new GetExpenseByIdQuery(expenseId));

            _logger.LogInformation("END: GetExpenseById");

            return Ok(new ApiOkResponse(expense));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPut("{expenseId:guid}")]
    [SwaggerOperation(
        Summary = "Update an existing expense",
        Description =
            "Updates the details of an existing expense based on the provided expense ID and update information."
    )]
    [SwaggerResponse(200, "Expense updated successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Expense with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.UPDATE)]
    public async Task<IActionResult> PutUpdateExpense(Guid expenseId, [FromBody] UpdateExpenseDto request)
    {
        _logger.LogInformation("START: PutUpdateExpense");
        try
        {
            await _mediator.Send(new UpdateExpenseCommand(expenseId, request));

            _logger.LogInformation("END: PutUpdateExpense");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpDelete("{expenseId:guid}")]
    [SwaggerOperation(
        Summary = "Delete a expense",
        Description = "Deletes the expense with the specified ID."
    )]
    [SwaggerResponse(200, "Expense deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Expense with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteExpense(Guid expenseId)
    {
        _logger.LogInformation("START: DeleteExpense");
        try
        {
            await _mediator.Send(new DeleteExpenseCommand(expenseId));

            _logger.LogInformation("END: DeleteExpense");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPost("{expenseId:guid}/sub-expenses")]
    [SwaggerOperation(
        Summary = "Create a new sub expense",
        Description = "Creates a new sub expense based on the provided details."
    )]
    [SwaggerResponse(201, "Sub expense created successfully", typeof(SubExpenseDto))]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.CREATE)]
    public async Task<IActionResult> PostCreateSubExpense(Guid expenseId, [FromBody] CreateSubExpenseDto request)
    {
        _logger.LogInformation("START: PostCreateSubExpense");

        SubExpenseDto subExpense = await _mediator.Send(new CreateSubExpenseCommand(expenseId, request));

        _logger.LogInformation("END: PostCreateSubExpense");

        return Ok(new ApiCreatedResponse(subExpense));
    }

    [HttpGet("{expenseId:guid}/sub-expenses")]
    [SwaggerOperation(
        Summary = "Retrieve a list of sub expenses by the expense",
        Description =
            "Fetches a paginated list of sub expenses created by the expense, based on the provided pagination filter."
    )]
    [SwaggerResponse(200, "Successfully retrieved the list of sub expenses", typeof(List<SubExpenseDto>))]
    [SwaggerResponse(404, "Not Found - Event with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.VIEW)]
    public async Task<IActionResult> GetSubExpensesByExpense(Guid expenseId)
    {
        _logger.LogInformation("START: GetSubExpensesByExpense");
        try
        {
            List<SubExpenseDto> expenses = await _mediator.Send(new GetSubExpensesByExpenseIdQuery(expenseId));

            _logger.LogInformation("END: GetSubExpensesByExpense");

            return Ok(new ApiOkResponse(expenses));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpPut("{expenseId:guid}/sub-expenses/{subExpenseId:guid}")]
    [SwaggerOperation(
        Summary = "Update an existing sub expense",
        Description =
            "Updates the details of an existing sub expense based on the provided sub expense ID and update information."
    )]
    [SwaggerResponse(200, "Sub expense updated successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Expense with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.UPDATE)]
    public async Task<IActionResult> PutUpdateSubExpense(Guid subExpenseId, [FromBody] UpdateSubExpenseDto request)
    {
        _logger.LogInformation("START: PutUpdateSubExpense");
        try
        {
            await _mediator.Send(new UpdateSubExpenseCommand(subExpenseId, request));

            _logger.LogInformation("END: PutUpdateSubExpense");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }

    [HttpDelete("{expenseId:guid}/sub-expenses/{subExpenseId:guid}")]
    [SwaggerOperation(
        Summary = "Delete a sub expense",
        Description = "Deletes the sub expense with the specified ID."
    )]
    [SwaggerResponse(200, "Sub expense deleted successfully")]
    [SwaggerResponse(401, "Unauthorized - User not authenticated")]
    [SwaggerResponse(403, "Forbidden - User does not have the required permissions")]
    [SwaggerResponse(404, "Not Found - Expense with the specified ID not found")]
    [SwaggerResponse(500, "Internal Server Error - An error occurred while processing the request")]
    [ClaimRequirement(EFunctionCode.GENERAL_EXPENSE, ECommandCode.DELETE)]
    public async Task<IActionResult> DeleteSubExpense(Guid subExpenseId)
    {
        _logger.LogInformation("START: DeleteSubExpense");
        try
        {
            await _mediator.Send(new DeleteSubExpenseCommand(subExpenseId));

            _logger.LogInformation("END: DeleteSubExpense");

            return Ok(new ApiOkResponse(true));
        }
        catch (NotFoundException e)
        {
            return NotFound(new ApiNotFoundResponse(e.Message));
        }
    }
}


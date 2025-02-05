using EventHub.Application.SeedWork.DTOs.Event;

namespace EventHub.Application.SeedWork.DTOs.Expense;

public class ExpenseDto
{
    public Guid Id { get; set; }

    public Guid EventId { get; set; } = Guid.Empty;

    public string Title { get; set; } = string.Empty;

    public long Total { get; set; }

    public LeanEventDto Event { get; set; } = null!;

    public List<SubExpenseDto> SubExpenses { get; set; } = new List<SubExpenseDto>();
}

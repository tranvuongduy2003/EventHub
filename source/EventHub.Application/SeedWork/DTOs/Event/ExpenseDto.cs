namespace EventHub.Application.SeedWork.DTOs.Event;

public class ExpenseDto
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = string.Empty;

    public long Total { get; set; }
    
    public List<SubExpenseDto> SubExpenses { get; set; } = new List<SubExpenseDto>();
}

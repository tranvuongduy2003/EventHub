using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Expense.CreateExpense;

public class CreateExpenseCommandHandler : ICommandHandler<CreateExpenseCommand, ExpenseDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateExpenseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExpenseDto> Handle(CreateExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = new Domain.Aggregates.EventAggregate.Entities.Expense
        {
            EventId = request.EventId,
            Total = request.Total,
            Title = request.Title,
        };

        await _unitOfWork.Expenses.CreateAsync(expense);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ExpenseDto>(expense);
    }
}

using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Expense.CreateSubExpense;

public class CreateSubExpenseCommandHandler : ICommandHandler<CreateSubExpenseCommand, SubExpenseDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateSubExpenseCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<SubExpenseDto> Handle(CreateSubExpenseCommand request, CancellationToken cancellationToken)
    {
        var subExpense = new Domain.Aggregates.EventAggregate.ValueObjects.SubExpense
        {
            ExpenseId = request.ExpenseId,
            Name = request.Name,
            Price = request.Price,
        };
        await _unitOfWork.SubExpenses.CreateAsync(subExpense);

        Domain.Aggregates.EventAggregate.Entities.Expense expense = await _unitOfWork.Expenses.GetByIdAsync(subExpense.ExpenseId);
        expense.Total += subExpense.Price;
        await _unitOfWork.Expenses.Update(expense);

        await _unitOfWork.CommitAsync();

        return _mapper.Map<SubExpenseDto>(subExpense);
    }
}

using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.EventAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Expense.DeleteExpense;

public class DeleteExpenseCommandHandler : ICommandHandler<DeleteExpenseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Entities.Expense expense = await _unitOfWork.Expenses.GetByIdAsync(request.Id);
        if (expense is null)
        {
            throw new NotFoundException("Expense does not exist!");
        }

        List<SubExpense> subExpenses = await _unitOfWork.SubExpenses
            .FindByCondition(x => x.ExpenseId == expense.Id)
            .ToListAsync(cancellationToken);
        await _unitOfWork.SubExpenses.DeleteList(subExpenses);

        await _unitOfWork.Expenses.Delete(expense);
        await _unitOfWork.CommitAsync();
    }
}

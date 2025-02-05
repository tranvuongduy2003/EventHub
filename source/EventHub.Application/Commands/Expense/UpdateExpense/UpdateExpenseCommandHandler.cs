using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Expense.UpdateExpense;

public class UpdateExpenseCommandHandler : ICommandHandler<UpdateExpenseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateExpenseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateExpenseCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Entities.Expense expense = await _unitOfWork.Expenses.GetByIdAsync(request.Id);
        if (expense is null)
        {
            throw new NotFoundException("Expense does not exist!");
        }

        expense.Title = request.Title;
        expense.Total = request.Total;

        await _unitOfWork.Expenses.Update(expense);
        await _unitOfWork.CommitAsync();
    }
}

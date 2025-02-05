using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Expense.UpdateSubExpense;

public class UpdateSubExpenseCommandHandler : ICommandHandler<UpdateSubExpenseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSubExpenseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateSubExpenseCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.ValueObjects.SubExpense subExpense = await _unitOfWork.SubExpenses.GetByIdAsync(request.Id);
        if (subExpense is null)
        {
            throw new NotFoundException("Sub expense does not exist!");
        }

        subExpense.Name = request.Name;
        subExpense.Price = request.Price;

        await _unitOfWork.SubExpenses.Update(subExpense);
        await _unitOfWork.CommitAsync();
    }
}

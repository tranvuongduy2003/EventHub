﻿using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Expense.DeleteSubExpense;

public class DeleteSubExpenseCommandHandler : ICommandHandler<DeleteSubExpenseCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSubExpenseCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteSubExpenseCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.ValueObjects.SubExpense subExpense = await _unitOfWork.SubExpenses.GetByIdAsync(request.Id);
        if (subExpense is null)
        {
            throw new NotFoundException("Sub expense does not exist!");
        }

        await _unitOfWork.SubExpenses.Delete(subExpense);
        await _unitOfWork.CommitAsync();
    }
}

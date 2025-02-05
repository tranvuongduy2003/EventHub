using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Expense.GetSubExpensesByExpenseId;

public class
    GetSubExpensesByExpenseIdQueryHandler : IQueryHandler<GetSubExpensesByExpenseIdQuery, List<SubExpenseDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetSubExpensesByExpenseIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<SubExpenseDto>> Handle(GetSubExpensesByExpenseIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isExpenseExisted = await _unitOfWork.Expenses.ExistAsync(x => x.Id == request.ExpenseId);
        if (!isExpenseExisted)
        {
            throw new NotFoundException("Expense does not exist!");
        }

        List<Domain.Aggregates.EventAggregate.ValueObjects.SubExpense> subExpenses = await _unitOfWork.SubExpenses
            .FindByCondition(x => x.ExpenseId == request.ExpenseId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<SubExpenseDto>>(subExpenses);
    }
}

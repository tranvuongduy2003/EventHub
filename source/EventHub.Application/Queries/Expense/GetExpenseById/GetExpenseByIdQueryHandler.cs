using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Expense.GetExpenseById;

public class GetExpenseByIdQueryHandler : IQueryHandler<GetExpenseByIdQuery, ExpenseDto>
{

    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetExpenseByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ExpenseDto> Handle(GetExpenseByIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Entities.Expense expense = await _unitOfWork.Expenses
            .FindByCondition(x => x.Id == request.ExpenseId)
            .Include(x => x.Event)
            .Include(x => x.SubExpenses)
            .FirstOrDefaultAsync(cancellationToken);

        if (expense == null)
        {
            throw new NotFoundException("Expense does not exist!");
        }

        ExpenseDto review = _mapper.Map<ExpenseDto>(expense);

        return review;
    }
}

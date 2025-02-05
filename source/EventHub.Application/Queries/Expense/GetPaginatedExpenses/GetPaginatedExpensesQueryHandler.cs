using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Expense.GetPaginatedExpenses;

public class GetPaginatedExpensesQueryHandler : IQueryHandler<GetPaginatedExpensesQuery, Pagination<ExpenseDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedExpensesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<ExpenseDto>> Handle(GetPaginatedExpensesQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.EventAggregate.Entities.Expense> paginatedExpenses = _unitOfWork.Expenses
            .PaginatedFind(request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.SubExpenses)
            );

        Pagination<ExpenseDto> paginatedExpenseDtos = _mapper.Map<Pagination<ExpenseDto>>(paginatedExpenses);

        return Task.FromResult(paginatedExpenseDtos);
    }
}

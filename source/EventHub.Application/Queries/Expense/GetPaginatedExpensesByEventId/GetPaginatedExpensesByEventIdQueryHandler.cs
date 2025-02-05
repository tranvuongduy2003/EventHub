using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Expense;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Expense.GetPaginatedExpensesByEventId;

public class
    GetPaginatedExpensesByEventIdQueryHandler : IQueryHandler<GetPaginatedExpensesByEventIdQuery, Pagination<ExpenseDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedExpensesByEventIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<ExpenseDto>> Handle(GetPaginatedExpensesByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id == request.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        Pagination<Domain.Aggregates.EventAggregate.Entities.Expense> paginatedExpenses = _unitOfWork.Expenses
            .PaginatedFindByCondition(x => x.EventId == request.EventId, request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.SubExpenses)
            );

        Pagination<ExpenseDto> paginatedExpenseDtos = _mapper.Map<Pagination<ExpenseDto>>(paginatedExpenses);

        return paginatedExpenseDtos;
    }
}

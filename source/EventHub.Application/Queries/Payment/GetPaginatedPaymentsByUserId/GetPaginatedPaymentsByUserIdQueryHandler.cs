using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Payment;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Payment.GetPaginatedPaymentsByUserId;

public class GetPaginatedPaymentsByUserIdQueryHandler : IQueryHandler<GetPaginatedPaymentsByUserIdQuery, Pagination<PaymentDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPaginatedPaymentsByUserIdQueryHandler(IUnitOfWork unitOfWork, UserManager<Domain.Aggregates.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Pagination<PaymentDto>> Handle(GetPaginatedPaymentsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user is null)
        {
            throw new NotFoundException("User does not exist!");
        }

        Pagination<Domain.Aggregates.PaymentAggregate.Payment> paginatedPayments = _unitOfWork.Payments
            .PaginatedFindByCondition(x => x.AuthorId == request.UserId, request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.Coupon)
                .Include(x => x.PaymentItems)
                .Include(x => x.Author)
            );

        Pagination<PaymentDto> paginatedPaymentDtos = _mapper.Map<Pagination<PaymentDto>>(paginatedPayments);

        return paginatedPaymentDtos;
    }
}

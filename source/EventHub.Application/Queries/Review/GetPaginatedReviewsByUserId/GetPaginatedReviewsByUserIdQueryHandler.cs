using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByUserId;

public class
    GetPaginatedReviewsByUserIdQueryHandler : IQueryHandler<GetPaginatedReviewsByUserIdQuery, Pagination<ReviewDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPaginatedReviewsByUserIdQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isUserExisted = await _userManager.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
        if (!isUserExisted)
        {
            throw new NotFoundException("User does not exist!");
        }

        Pagination<Domain.Aggregates.EventAggregate.Entities.Review> paginatedReviews = _unitOfWork.CachedReviews
            .PaginatedFindByCondition(x => x.AuthorId == request.UserId, request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.Author)
            );

        Pagination<ReviewDto> paginatedReviewDtos = _mapper.Map<Pagination<ReviewDto>>(paginatedReviews);

        return paginatedReviewDtos;
    }
}

using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Review;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByUserId;

public class
    GetPaginatedReviewsByUserIdQueryHandler : IQueryHandler<GetPaginatedReviewsByUserIdQuery, Pagination<ReviewDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetPaginatedReviewsByUserIdQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var isUserExisted = await _userManager.Users.AnyAsync(x => x.Id.Equals(request.UserId));
        if (!isUserExisted)
            throw new NotFoundException("User does not exist!");

        var reviews = await _unitOfWork.CachedReviews
            .FindByCondition(x => x.AuthorId.Equals(request.UserId))
            .Include(x => x.Event)
            .Include(x => x.Author)
            .ToListAsync();

        var reviewDtos = _mapper.Map<List<ReviewDto>>(reviews);


        return PagingHelper.Paginate<ReviewDto>(reviewDtos, request.Filter);
    }
}
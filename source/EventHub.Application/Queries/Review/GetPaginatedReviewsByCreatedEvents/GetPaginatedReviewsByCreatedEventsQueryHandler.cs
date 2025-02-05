using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByCreatedEvents;

public class GetPaginatedReviewsByCreatedEventsQueryHandler : IQueryHandler<GetPaginatedReviewsByCreatedEventsQuery, Pagination<ReviewDto>>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedReviewsByCreatedEventsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsByCreatedEventsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_signInManager.Context.User.Identities.FirstOrDefault()?
            .FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "");

        Pagination<Domain.Aggregates.EventAggregate.Entities.Review> paginatedReviews = _unitOfWork.CachedReviews
            .PaginatedFind(request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.Author)
                .Where(x => x.Event.AuthorId == userId)
            );

        Pagination<ReviewDto> paginatedReviewDtos = _mapper.Map<Pagination<ReviewDto>>(paginatedReviews);

        return Task.FromResult(paginatedReviewDtos);
    }
}

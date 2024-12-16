using AutoMapper;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedFollowers;

public class GetPaginatedFollowersQueryHandler : IQueryHandler<GetPaginatedFollowersQuery, Pagination<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedFollowersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<UserDto>> Handle(GetPaginatedFollowersQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.UserAggregate.User> paginatedFollowers = PagingHelper.QueryPaginate(
            request.Filter,
            _unitOfWork.UserFollowers
                .FindByCondition(x => x.FollowedId == request.UserId)
                .Include(x => x.Follower)
                .Select(x => x.Follower));

        Pagination<UserDto> paginatedFollowerDtos = _mapper.Map<Pagination<UserDto>>(paginatedFollowers);

        return Task.FromResult(paginatedFollowerDtos);
    }
}

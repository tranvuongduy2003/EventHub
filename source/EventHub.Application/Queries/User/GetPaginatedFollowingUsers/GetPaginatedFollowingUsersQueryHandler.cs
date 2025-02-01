using AutoMapper;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedFollowingUsers;

public class GetPaginatedFollowingUsersQueryHandler : IQueryHandler<GetPaginatedFollowingUsersQuery, Pagination<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedFollowingUsersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<UserDto>> Handle(GetPaginatedFollowingUsersQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.UserAggregate.User> paginatedFollowingUsers = PagingHelper.QueryPaginate(
            request.Filter,
            _unitOfWork.UserFollowers
                .FindByCondition(x => x.FollowerId == request.UserId)
                .Include(x => x.Followed)
                .Select(x => x.Followed));

        Pagination<UserDto> paginatedFollowingUserDtos = _mapper.Map<Pagination<UserDto>>(paginatedFollowingUsers);

        return Task.FromResult(paginatedFollowingUserDtos);
    }
}

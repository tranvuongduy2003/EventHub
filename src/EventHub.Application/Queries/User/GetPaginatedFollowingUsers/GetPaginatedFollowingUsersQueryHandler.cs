using AutoMapper;
using EventHub.Abstractions;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedFollowingUsers;

public class
    GetPaginatedFollowingUsersQueryHandler : IQueryHandler<GetPaginatedFollowingUsersQuery, Pagination<UserDto>>
{
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetPaginatedFollowingUsersQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ICacheService cacheService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> Handle(GetPaginatedFollowingUsersQuery request,
        CancellationToken cancellationToken)
    {
        var key = $"user:following:{request.UserId}";

        var users = await _cacheService.GetData<List<Domain.AggregateModels.UserAggregate.User>>(key);

        if (users == null || !users.Any())
        {
            users = await _userManager.Users
                .AsNoTracking()
                .Where(x => x.IsDeleted.Equals(false))
                .ToListAsync();

            await _cacheService.SetData<List<Domain.AggregateModels.UserAggregate.User>>(key, users,
                TimeSpan.FromMinutes(2));
        }

        var followingUsers = await _unitOfWork.UserFollowers
            .FindByCondition(x => x.FollowerId.Equals(request.UserId))
            .Join(users, userFollower => userFollower.FollowedId, user => user.Id, (_, user) => user)
            .ToListAsync();

        var followingUserDtos = _mapper.Map<List<UserDto>>(followingUsers);


        return PagingHelper.Paginate<UserDto>(followingUserDtos, request.Filter);
    }
}
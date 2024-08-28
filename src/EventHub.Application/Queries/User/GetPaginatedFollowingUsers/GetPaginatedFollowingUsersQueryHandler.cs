using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.User.GetPaginatedFollowingUsers;

public class GetPaginatedFollowingUsersQueryHandler : IQueryHandler<GetPaginatedFollowingUsersQuery, Pagination<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetPaginatedFollowingUsersQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetPaginatedFollowingUsersQueryHandler(IUnitOfWork unitOfWork, UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ICacheService cacheService,
        ILogger<GetPaginatedFollowingUsersQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _cacheService = cacheService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> Handle(GetPaginatedFollowingUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPaginatedFollowingUsersQueryHandler");
        
        var key = "User";

        var users = await _cacheService.GetData<List<Domain.AggregateModels.UserAggregate.User>>(key);

        if (users == null || !users.Any())
        {
            users = await _userManager.Users
                .AsNoTracking()
                .Where(x => x.IsDeleted.Equals(false))
                .ToListAsync();

            await _cacheService.SetData<List<Domain.AggregateModels.UserAggregate.User>>(key, users, TimeSpan.FromMinutes(2));
        }

        var followingUsers = await _unitOfWork.UserFollowers
            .FindByCondition(x => x.FollowerId.Equals(request.UserId))
            .Join(users, userFollower => userFollower.FollowedId, user => user.Id, (_, user) => user)
            .ToListAsync();
        
        var followingUserDtos = _mapper.Map<List<UserDto>>(followingUsers);

        _logger.LogInformation("END: GetPaginatedFollowingUsersQueryHandler");

        return PagingHelper.Paginate<UserDto>(followingUserDtos, request.Filter);
    }
}
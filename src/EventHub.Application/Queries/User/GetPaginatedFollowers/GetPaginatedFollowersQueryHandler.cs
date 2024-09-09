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

namespace EventHub.Application.Queries.User.GetPaginatedFollowers;

public class GetPaginatedFollowersQueryHandler : IQueryHandler<GetPaginatedFollowersQuery, Pagination<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetPaginatedFollowersQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetPaginatedFollowersQueryHandler(IUnitOfWork unitOfWork, UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ICacheService cacheService,
        ILogger<GetPaginatedFollowersQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _cacheService = cacheService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> Handle(GetPaginatedFollowersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPaginatedFollowersQueryHandler");
        
        var key = $"user:follower:{request.UserId}";

        var users = await _cacheService.GetData<List<Domain.AggregateModels.UserAggregate.User>>(key);

        if (users == null || !users.Any())
        {
            users = await _userManager.Users
                .AsNoTracking()
                .Where(x => x.IsDeleted.Equals(false))
                .ToListAsync();

            await _cacheService.SetData<List<Domain.AggregateModels.UserAggregate.User>>(key, users, TimeSpan.FromMinutes(2));
        }

        var followers = await _unitOfWork.UserFollowers
            .FindByCondition(x => x.FollowedId.Equals(request.UserId))
            .Join(users, userFollower => userFollower.FollowerId, user => user.Id, (_, user) => user)
            .ToListAsync();
        
        var followerDtos = _mapper.Map<List<UserDto>>(followers);

        _logger.LogInformation("END: GetPaginatedFollowersQueryHandler");

        return PagingHelper.Paginate<UserDto>(followerDtos, request.Filter);
    }
}
using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Abstractions.Services;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedFollowers;

public class GetPaginatedFollowersQueryHandler : IQueryHandler<GetPaginatedFollowersQuery, Pagination<UserDto>>
{
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetPaginatedFollowersQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ICacheService cacheService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> Handle(GetPaginatedFollowersQuery request,
        CancellationToken cancellationToken)
    {
        string key = $"user:follower:{request.UserId}";

        List<Domain.AggregateModels.UserAggregate.User> users = await _cacheService.GetData<List<Domain.AggregateModels.UserAggregate.User>>(key);

        if (users == null || !users.Any())
        {
            users = await _userManager.Users
                .AsNoTracking()
                .Where(x => x.IsDeleted.Equals(false))
                .ToListAsync(cancellationToken);

            await _cacheService.SetData<List<Domain.AggregateModels.UserAggregate.User>>(key, users,
                TimeSpan.FromMinutes(2));
        }

        List<Domain.AggregateModels.UserAggregate.User> followers = await _unitOfWork.UserFollowers
            .FindByCondition(x => x.FollowedId.Equals(request.UserId))
            .Join(users, userFollower => userFollower.FollowerId, user => user.Id, (_, user) => user)
            .ToListAsync(cancellationToken);

        List<UserDto> followerDtos = _mapper.Map<List<UserDto>>(followers);

        return PagingHelper.Paginate<UserDto>(followerDtos, request.Filter);
    }
}

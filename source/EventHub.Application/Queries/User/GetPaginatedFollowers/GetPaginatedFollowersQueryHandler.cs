using AutoMapper;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedFollowers;

public class GetPaginatedFollowersQueryHandler : IQueryHandler<GetPaginatedFollowersQuery, Pagination<UserDto>>
{
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPaginatedFollowersQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, ICacheService cacheService, IMapper mapper)
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

        List<Domain.Aggregates.UserAggregate.User> users = await _cacheService.GetData<List<Domain.Aggregates.UserAggregate.User>>(key);

        if (users == null || !users.Any())
        {
            users = await _userManager.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .ToListAsync(cancellationToken);

            await _cacheService.SetData<List<Domain.Aggregates.UserAggregate.User>>(key, users,
                TimeSpan.FromMinutes(2));
        }

        List<Domain.Aggregates.UserAggregate.User> followers = await _unitOfWork.UserFollowers
            .FindByCondition(x => x.FollowedId == request.UserId)
            .Join(users, userFollower => userFollower.FollowerId, user => user.Id, (_, user) => user)
            .ToListAsync(cancellationToken);

        List<UserDto> followerDtos = _mapper.Map<List<UserDto>>(followers);

        return PagingHelper.Paginate<UserDto>(followerDtos, request.Filter);
    }
}

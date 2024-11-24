using AutoMapper;
using EventHub.Abstractions.Services;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedUsers;

public class GetPaginatedUsersQueryHandler : IQueryHandler<GetPaginatedUsersQuery, Pagination<UserDto>>
{
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetPaginatedUsersQueryHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        ICacheService cacheService, IMapper mapper)
    {
        _userManager = userManager;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> Handle(GetPaginatedUsersQuery request,
        CancellationToken cancellationToken)
    {
        string key = "user";

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

        List<UserDto> userDtos = _mapper.Map<List<UserDto>>(users);

        return PagingHelper.Paginate<UserDto>(userDtos, request.Filter);
    }
}

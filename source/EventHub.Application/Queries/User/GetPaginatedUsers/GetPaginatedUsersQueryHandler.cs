using AutoMapper;
using EventHub.Application.SeedWork.Abstractions;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedUsers;

public class GetPaginatedUsersQueryHandler : IQueryHandler<GetPaginatedUsersQuery, Pagination<UserDto>>
{
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPaginatedUsersQueryHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager,
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

        List<UserDto> userDtos = _mapper.Map<List<UserDto>>(users);

        return PagingHelper.Paginate<UserDto>(userDtos, request.Filter);
    }
}

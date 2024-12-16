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

        IQueryable<Domain.Aggregates.UserAggregate.User> queryableUsers =
            await _cacheService.GetData<IQueryable<Domain.Aggregates.UserAggregate.User>>(key);

        if (queryableUsers == null || !queryableUsers.Any())
        {
            queryableUsers = _userManager.Users
                .AsNoTracking()
                .Where(x => !x.IsDeleted);

            await _cacheService.SetData(key, queryableUsers, TimeSpan.FromMinutes(2));
        }

        Pagination<Domain.Aggregates.UserAggregate.User> paginatedUsers =
            PagingHelper.QueryPaginate(request.Filter, queryableUsers);

        Pagination<UserDto> paginatedUserDtos = _mapper.Map<Pagination<UserDto>>(paginatedUsers);

        return paginatedUserDtos;
    }
}

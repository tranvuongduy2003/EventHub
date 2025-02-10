using AutoMapper;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedUsers;

public class GetPaginatedUsersQueryHandler : IQueryHandler<GetPaginatedUsersQuery, Pagination<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetPaginatedUsersQueryHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> Handle(GetPaginatedUsersQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.Aggregates.UserAggregate.User> users = await _userManager.Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        Pagination<Domain.Aggregates.UserAggregate.User> paginatedUsers =
            PagingHelper.QueryPaginate(request.Filter, users.AsQueryable());
        paginatedUsers.Items.ForEach(x =>
        {
            x.Roles = _userManager
                .GetRolesAsync(x)
                .GetAwaiter()
                .GetResult()
                .Select(role => new Domain.Aggregates.UserAggregate.Entities.Role(role))
                .ToList();
        });

        Pagination<UserDto> paginatedUserDtos = _mapper.Map<Pagination<UserDto>>(paginatedUsers);

        return paginatedUserDtos;
    }
}

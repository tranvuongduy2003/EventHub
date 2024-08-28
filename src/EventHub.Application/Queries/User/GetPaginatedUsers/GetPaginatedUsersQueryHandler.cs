using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.User.GetPaginatedUsers;

public class GetPaginatedUsersQueryHandler : IQueryHandler<GetPaginatedUsersQuery, Pagination<UserDto>>
{
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly ICacheService _cacheService;
    private readonly ILogger<GetPaginatedUsersQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetPaginatedUsersQueryHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ICacheService cacheService,
        ILogger<GetPaginatedUsersQueryHandler> logger, IMapper mapper)
    {
        _userManager = userManager;
        _cacheService = cacheService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<UserDto>> Handle(GetPaginatedUsersQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetPaginatedUsersQueryHandler");
        
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
        
        var userDtos = _mapper.Map<List<UserDto>>(users);

        _logger.LogInformation("END: GetPaginatedUsersQueryHandler");

        return PagingHelper.Paginate<UserDto>(userDtos, request.Filter);
    }
}
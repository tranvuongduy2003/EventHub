using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.User.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        ILogger<GetUserByIdQueryHandler> logger, ICacheService cacheService, IMapper mapper)
    {
        _userManager = userManager;
        _logger = logger;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetUserByIdQueryHandler");

        string key = $"user:{request.UserId}";

        var user = await _cacheService.GetData<Domain.AggregateModels.UserAggregate.User>(key);

        if (user == null)
        {
            user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                throw new NotFoundException("User does not exist!");
            await _cacheService.SetData<Domain.AggregateModels.UserAggregate.User>(key, user, TimeSpan.FromMinutes(2));
        }

        var userDto = _mapper.Map<UserDto>(user);

        _logger.LogInformation("END: GetUserByIdQueryHandler");

        return userDto;
    }
}
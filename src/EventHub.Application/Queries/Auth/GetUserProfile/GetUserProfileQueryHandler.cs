using AutoMapper;
using EventHub.Abstractions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserDto>
{
    private readonly ILogger<GetUserProfileQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetUserProfileQueryHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        ITokenService tokenService, IMapper mapper, ILogger<GetUserProfileQueryHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetUserProfileQueryHandler");

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) throw new UnauthorizedException("Unauthorized");

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles.ToList();

        _logger.LogInformation("END: GetUserProfileQueryHandler");

        return userDto;
    }
}
using AutoMapper;
using EventHub.Domain.Abstractions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using EventHub.Shared.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

public class GetUserProfileQueryHandler: IQueryHandler<GetUserProfileQuery, UserDto>
{
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;
    private readonly ILogger<GetUserProfileQueryHandler> _logger;

    public GetUserProfileQueryHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager, ITokenService tokenService, IMapper mapper, ILogger<GetUserProfileQueryHandler> logger)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetUserProfileQueryHandler");
        
        if (string.IsNullOrEmpty(request.AccessToken)) throw new UnauthorizedException("Unauthorized");
        var principal = _tokenService.GetPrincipalFromToken(request.AccessToken);

        var user = await _userManager.FindByIdAsync(principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
        if (user == null) throw new UnauthorizedException("Unauthorized");

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles.ToList();
        
        _logger.LogInformation("END: GetUserProfileQueryHandler");

        return userDto;
    }
}
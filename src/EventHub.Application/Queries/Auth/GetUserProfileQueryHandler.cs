using AutoMapper;
using EventHub.Domain.AggregateModels.UserAggregate;
using EventHub.Domain.Services;
using EventHub.Shared.Exceptions;
using EventHub.Shared.Models.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.Auth;

public class GetUserProfileQueryHandler: IRequestHandler<GetUserProfileQuery, UserModel>
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public GetUserProfileQueryHandler(UserManager<User> userManager, ITokenService tokenService, IMapper mapper)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _mapper = mapper;
    }
    
    public async Task<UserModel> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.AccessToken)) throw new UnauthorizedException("Unauthorized");
        var principal = _tokenService.GetPrincipalFromToken(request.AccessToken);

        var user = await _userManager.FindByIdAsync(principal.Claims
            .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti).Value);
        if (user == null) throw new UnauthorizedException("Unauthorized");

        var roles = await _userManager.GetRolesAsync(user);
        var userModel = _mapper.Map<UserModel>(user);
        userModel.Roles = roles.ToList();

        return userModel;
    }
}
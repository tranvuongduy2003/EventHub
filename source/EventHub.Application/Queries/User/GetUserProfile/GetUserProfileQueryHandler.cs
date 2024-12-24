using AutoMapper;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.User.GetUserProfile;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserDto>
{
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public GetUserProfileQueryHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new UnauthorizedException("Unauthorized");
        }

        IList<string> roles = await _userManager.GetRolesAsync(user);
        UserDto userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles.ToList();

        return userDto;
    }
}

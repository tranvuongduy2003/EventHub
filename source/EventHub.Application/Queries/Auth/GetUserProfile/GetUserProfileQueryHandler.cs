using AutoMapper;
using EventHub.Application.DTOs.User;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserDto>
{
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetUserProfileQueryHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.UserAggregate.User user = await _userManager.FindByIdAsync(request.UserId.ToString());
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

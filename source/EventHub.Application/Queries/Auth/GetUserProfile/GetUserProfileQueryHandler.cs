using AutoMapper;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Queries.Auth.GetUserProfile;

public class GetUserProfileQueryHandler : IQueryHandler<GetUserProfileQuery, UserDto>
{
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetUserProfileQueryHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null) throw new UnauthorizedException("Unauthorized");

        var roles = await _userManager.GetRolesAsync(user);
        var userDto = _mapper.Map<UserDto>(user);
        userDto.Roles = roles.ToList();

        return userDto;
    }
}
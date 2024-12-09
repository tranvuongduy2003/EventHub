using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.User;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Queries.User.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetUserByIdQueryHandler(UserManager<Domain.Aggregates.UserAggregate.User> userManager,
        ICacheService cacheService, IMapper mapper)
    {
        _userManager = userManager;
        _cacheService = cacheService;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        string key = $"user:{request.UserId}";

        Domain.Aggregates.UserAggregate.User user = await _cacheService.GetData<Domain.Aggregates.UserAggregate.User>(key);

        if (user == null)
        {
            user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                throw new NotFoundException("User does not exist!");
            }
            await _cacheService.SetData<Domain.Aggregates.UserAggregate.User>(key, user, TimeSpan.FromMinutes(2));
        }

        UserDto userDto = _mapper.Map<UserDto>(user);

        return userDto;
    }
}

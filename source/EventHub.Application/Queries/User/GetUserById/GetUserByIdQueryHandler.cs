using AutoMapper;
using EventHub.Abstractions;
using EventHub.Abstractions.Services;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.User;
using Microsoft.AspNetCore.Identity;

namespace EventHub.Application.Queries.User.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly ICacheService _cacheService;
    private readonly IMapper _mapper;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetUserByIdQueryHandler(UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
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

        var user = await _cacheService.GetData<Domain.AggregateModels.UserAggregate.User>(key);

        if (user == null)
        {
            user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
                throw new NotFoundException("User does not exist!");
            await _cacheService.SetData<Domain.AggregateModels.UserAggregate.User>(key, user, TimeSpan.FromMinutes(2));
        }

        var userDto = _mapper.Map<UserDto>(user);


        return userDto;
    }
}
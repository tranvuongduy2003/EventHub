using AutoMapper;
using EventHub.Application.SeedWork.DTOs.User;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.User.GetPaginatedFollowersToInvite;

public class GetPaginatedFollowersToInviteQueryHandler : IQueryHandler<GetPaginatedFollowersToInviteQuery, Pagination<UserDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedFollowersToInviteQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<UserDto>> Handle(GetPaginatedFollowersToInviteQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.UserAggregate.User> paginatedFollowers = PagingHelper.QueryPaginate(
            request.Filter,
            _unitOfWork.UserFollowers
                .FindByCondition(x => x.FollowedId == request.UserId)
                .Include(x => x.Follower)
                    .ThenInclude(x => x.Inviteds)
                .Select(x => x.Follower));

        Pagination<UserDto> paginatedFollowerDtos = _mapper.Map<Pagination<UserDto>>(paginatedFollowers);

        for (int i = 0; i < paginatedFollowerDtos.Items.Count; i++)
        {
            paginatedFollowerDtos.Items[i].IsInvited = paginatedFollowers.Items[i].Inviteds.Any(x => x.InviterId == request.UserId && x.EventId == request.EventId);
        }

        return Task.FromResult(paginatedFollowerDtos);
    }
}

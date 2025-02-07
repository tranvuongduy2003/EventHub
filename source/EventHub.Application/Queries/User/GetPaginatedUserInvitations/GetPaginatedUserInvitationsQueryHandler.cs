using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Invitation;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;

namespace EventHub.Application.Queries.User.GetPaginatedUserInvitations;

public class GetPaginatedUserInvitationsQueryHandler : IQueryHandler<GetPaginatedUserInvitationsQuery, Pagination<InvitationDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;

    public GetPaginatedUserInvitationsQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _signInManager = signInManager;
    }

    public Task<Pagination<InvitationDto>> Handle(GetPaginatedUserInvitationsQuery request,
        CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        Pagination<Domain.Aggregates.UserAggregate.ValueObjects.Invitation> paginatedInvitations = _unitOfWork.Invitations
            .PaginatedFindByCondition(x => x.InvitedId == Guid.Parse(userId), request.Filter, query => query
                .Include(x => x.Invited)
                .Include(x => x.Inviter)
                .Include(x => x.Event));

        Pagination<InvitationDto> paginatedInvitationDtos = _mapper.Map<Pagination<InvitationDto>>(paginatedInvitations);

        return Task.FromResult(paginatedInvitationDtos);
    }
}

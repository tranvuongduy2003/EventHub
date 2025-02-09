using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Conversation.GetConversationsByOrganizerId;

public class GetConversationsByOrganizerIdQueryHandler : IQueryHandler<GetConversationsByOrganizerIdQuery, Pagination<ConversationDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetConversationsByOrganizerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<ConversationDto>> Handle(GetConversationsByOrganizerIdQuery request,
        CancellationToken cancellationToken)
    {
        Pagination<Domain.Aggregates.ConversationAggregate.Conversation> paginatedConversations = _unitOfWork
            .Conversations
            .PaginatedFindByCondition(
                x => x.HostId == request.OrganizerId,
                request.Filter,
                query => query
                    .Include(x => x.Event)
                    .Include(x => x.Host)
                    .Include(x => x.User)
                    .Include(x => x.Messages)
            );

        paginatedConversations.Items
            .ForEach(conversation =>
            {
                conversation.LastMessage = conversation.Messages?.MaxBy(x => x.CreatedAt);
            });

        Pagination<ConversationDto> paginatedConversationDtos =
            _mapper.Map<Pagination<ConversationDto>>(paginatedConversations);

        return Task.FromResult(paginatedConversationDtos);
    }
}

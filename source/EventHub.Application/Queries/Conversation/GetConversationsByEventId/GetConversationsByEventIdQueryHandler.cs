using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Conversation.GetConversationsByEventId;

public class
    GetConversationsByEventIdQueryHandler : IQueryHandler<GetConversationsByEventIdQuery, Pagination<ConversationDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetConversationsByEventIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<ConversationDto>> Handle(GetConversationsByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id == request.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        Pagination<Domain.Aggregates.ConversationAggregate.Conversation> paginatedConversations = _unitOfWork
            .Conversations
            .PaginatedFindByCondition(
                x => x.EventId == request.EventId,
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

        return paginatedConversationDtos;
    }
}

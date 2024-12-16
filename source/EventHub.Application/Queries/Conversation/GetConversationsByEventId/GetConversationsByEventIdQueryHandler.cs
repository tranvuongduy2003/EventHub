using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

        IIncludableQueryable<Message, Domain.Aggregates.UserAggregate.User> messages = _unitOfWork.Messages
            .FindAll()
            .Include(x => x.Author);

        Pagination<Domain.Aggregates.ConversationAggregate.Conversation> paginatedConversations = _unitOfWork
            .Conversations
            .PaginatedFindByCondition(
                x => x.EventId == request.EventId,
                request.Filter,
                query => query
                    .Include(x => x.Event)
                    .Include(x => x.Host)
                    .Include(x => x.User)
            );

        var conversationWithMessages = (
                from _conversation in paginatedConversations.Items
                join _message in messages.DefaultIfEmpty()
                    on _conversation.Id equals _message.ConversationId
                select new { Conversation = _conversation, Message = _message }
            )
            .GroupBy(x => x.Conversation)
            .AsEnumerable()
            .Select(group =>
            {
                Domain.Aggregates.ConversationAggregate.Conversation conversation = group.Key;
                conversation.LastMessage = group.MaxBy(x => x.Message.CreatedAt)?.Message;
                return conversation;
            })
            .ToList();

        Pagination<ConversationDto> paginatedConversationDtos =
            _mapper.Map<Pagination<ConversationDto>>(conversationWithMessages);

        return paginatedConversationDtos;
    }
}

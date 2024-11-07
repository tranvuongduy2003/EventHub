using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Conversation;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
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
        var isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id.Equals(request.EventId));
        if (!isEventExisted)
            throw new NotFoundException("Event does not exist!");

        var messages = _unitOfWork.Messages
            .FindAll()
            .Include(x => x.Author);

        var conversations = _unitOfWork.Conversations
            .FindByCondition(x => x.EventId.Equals(request.EventId))
            .Include(x => x.Event)
            .Include(x => x.Host)
            .Include(x => x.User)
            .ToList();

        var conversationWithMessages = (
                from _conversation in conversations
                join _message in messages.DefaultIfEmpty()
                    on _conversation.Id equals _message.ConversationId
                select new { Conversation = _conversation, Message = _message }
            )
            .GroupBy(x => x.Conversation)
            .AsEnumerable()
            .Select(group =>
            {
                var conversation = group.Key;
                conversation.LastMessage = group.MaxBy(x => x.Message.CreatedAt)?.Message;
                return conversation;
            })
            .ToList();

        var conversationDtos = _mapper.Map<List<ConversationDto>>(conversationWithMessages);


        return PagingHelper.Paginate<ConversationDto>(conversationDtos, request.Filter);
    }
}
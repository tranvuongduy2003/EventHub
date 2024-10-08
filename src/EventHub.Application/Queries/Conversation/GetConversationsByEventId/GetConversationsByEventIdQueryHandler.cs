﻿using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Conversation;
using EventHub.Shared.Exceptions;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Conversation.GetConversationsByEventId;

public class
    GetConversationsByEventIdQueryHandler : IQueryHandler<GetConversationsByEventIdQuery, Pagination<ConversationDto>>
{
    private readonly ILogger<GetConversationsByEventIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetConversationsByEventIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetConversationsByEventIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<ConversationDto>> Handle(GetConversationsByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetConversationsByEventIdQueryHandler");

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
            .LeftJoin(
                messages,
                _conversation => _conversation.Id,
                _message => _message.ConversationId,
                (_conversation, _message) => new { Conversation = _conversation, Message = _message })
            .GroupBy(x => x.Conversation)
            .AsEnumerable()
            .Select(group =>
            {
                var conversation = group.Key;
                conversation.LastMessage = group.MaxBy(x => x.Message.CreatedAt)?.Message;
                return conversation;
            })
            .ToList();

        var conversationDtos = _mapper.Map<List<ConversationDto>>(conversations);

        _logger.LogInformation("END: GetConversationsByEventIdQueryHandler");

        return PagingHelper.Paginate<ConversationDto>(conversationDtos, request.Filter);
    }
}
using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Conversation;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Conversation.GetConversationsByUserId;

public class
    GetConversationsByUserIdQueryHandler : IQueryHandler<GetConversationsByUserIdQuery, Pagination<ConversationDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetConversationsByUserIdQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;

        _mapper = mapper;
    }

    public async Task<Pagination<ConversationDto>> Handle(GetConversationsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var isUserExisted = await _userManager.Users.AnyAsync(x => x.Id.Equals(request.UserId));
        if (!isUserExisted)
            throw new NotFoundException("User does not exist!");


        var messages = _unitOfWork.Messages
            .FindAll()
            .Include(x => x.Author);

        var conversations = _unitOfWork.Conversations
            .FindByCondition(x => x.EventId.Equals(request.UserId))
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

        var conversationDtos = _mapper.Map<List<ConversationDto>>(conversations);


        return PagingHelper.Paginate<ConversationDto>(conversationDtos, request.Filter);
    }
}
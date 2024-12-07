using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Conversation.GetConversationsByUserId;

public class
    GetConversationsByUserIdQueryHandler : IQueryHandler<GetConversationsByUserIdQuery, Pagination<ConversationDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.Aggregates.UserAggregate.User> _userManager;

    public GetConversationsByUserIdQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.Aggregates.UserAggregate.User> userManager, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<Pagination<ConversationDto>> Handle(GetConversationsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isUserExisted = await _userManager.Users.AnyAsync(x => x.Id.Equals(request.UserId), cancellationToken);
        if (!isUserExisted)
        {
            throw new NotFoundException("User does not exist!");
        }

        List<Message> messages = await _unitOfWork.Messages
            .FindAll()
            .Include(x => x.Author)
            .ToListAsync(cancellationToken);

        List<Domain.Aggregates.ConversationAggregate.Conversation> conversations = await _unitOfWork.Conversations
            .FindByCondition(x => x.EventId.Equals(request.UserId))
            .Include(x => x.Event)
            .Include(x => x.Host)
            .Include(x => x.User)
            .ToListAsync(cancellationToken);

        var conversationsWithMessages = (
                from _conversation in conversations
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

        List<ConversationDto> conversationDtos = _mapper.Map<List<ConversationDto>>(conversationsWithMessages);

        return PagingHelper.Paginate<ConversationDto>(conversationDtos, request.Filter);
    }
}

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
        bool isUserExisted = await _userManager.Users.AnyAsync(x => x.Id == request.UserId, cancellationToken);
        if (!isUserExisted)
        {
            throw new NotFoundException("User does not exist!");
        }

        List<Message> messages = await _unitOfWork.Messages
            .FindAll()
            .Include(x => x.Author)
            .ToListAsync(cancellationToken);

        Pagination<Domain.Aggregates.ConversationAggregate.Conversation> conversations = _unitOfWork.Conversations
            .PaginatedFindByCondition(x => x.EventId == request.UserId, request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.Host)
                .Include(x => x.User));

        var conversationsWithMessages = (
                from _conversation in conversations.Items
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
            _mapper.Map<Pagination<ConversationDto>>(conversationsWithMessages);

        return paginatedConversationDtos;
    }
}

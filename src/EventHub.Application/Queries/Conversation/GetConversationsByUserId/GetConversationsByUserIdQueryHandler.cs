using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Conversation;
using EventHub.Shared.Exceptions;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Conversation.GetConversationsByUserId;

public class
    GetConversationsByUserIdQueryHandler : IQueryHandler<GetConversationsByUserIdQuery, Pagination<ConversationDto>>
{
    private readonly ILogger<GetConversationsByUserIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Domain.AggregateModels.UserAggregate.User> _userManager;

    public GetConversationsByUserIdQueryHandler(IUnitOfWork unitOfWork,
        UserManager<Domain.AggregateModels.UserAggregate.User> userManager,
        ILogger<GetConversationsByUserIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<ConversationDto>> Handle(GetConversationsByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetConversationsByUserIdQueryHandler");

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

        _logger.LogInformation("END: GetConversationsByUserIdQueryHandler");

        return PagingHelper.Paginate<ConversationDto>(conversationDtos, request.Filter);
    }
}
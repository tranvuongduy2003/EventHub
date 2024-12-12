using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Application.SeedWork.DTOs.Message;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.EventAggregate;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SignalRSwaggerGen.Attributes;

namespace EventHub.Infrastructure.SignalR.Hubs;

[SignalRHub]
public class ChatHub : Hub
{
    private static readonly List<Guid> _connections = new List<Guid>();
    private readonly ILogger<ChatHub> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    private readonly UserManager<User> _userManager;

    public ChatHub(ILogger<ChatHub> logger, IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task TestConnection()
    {
        _logger.LogInformation("BEGIN: TestConnection");

        await Clients.All.SendAsync("TestConnection", "Connect successfully!");

        _logger.LogInformation("END: TestConnection");
    }

    public async Task JoinChatRoom(JoinChatRoomDto request)
    {
        _logger.LogInformation("BEGIN: JoinChatRoom");

        Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
        if (@event == null)
        {
            throw new NotFoundException("Event does not exist");
        }

        if (@event.AuthorId != request.HostId)
        {
            throw new BadRequestException("Host does not belong to the event");
        }

        User user = await _userManager.FindByIdAsync(request.UserId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User does not exist");
        }

        User host = await _userManager.FindByIdAsync(request.HostId.ToString());
        if (host == null)
        {
            throw new NotFoundException("Host does not exist");
        }

        Conversation conversation = await _unitOfWork.Conversations
            .FindByCondition(x =>
                x.HostId == request.HostId
                && x.EventId == request.EventId
                && x.UserId == request.UserId)
            .Include(x => x.Event)
            .Include(x => x.Host)
            .Include(x => x.User)
            .FirstOrDefaultAsync();

        if (conversation == null)
        {
            var createdConversation = new Conversation
            {
                EventId = request.EventId,
                UserId = request.UserId,
                HostId = request.HostId,
            };

            await _unitOfWork.Conversations.CreateAsync(createdConversation);
            await _unitOfWork.CommitAsync();

            ConversationDto conversationDto = _mapper.Map<ConversationDto>(conversation);

            _logger.LogInformation("JoinChatRoom: Created Context Connection id: {ConnectionId}", Context.ConnectionId);

            await Groups.AddToGroupAsync(Context.ConnectionId, createdConversation.Id.ToString());

            _connections.Add(createdConversation.Id);

            await Clients.Group(createdConversation.Id.ToString()).SendAsync("JoinChatRoom", conversationDto,
                $"{user.FullName} has created  conversation {createdConversation.Id}");
        }
        else
        {
            ConversationDto conversationDto = _mapper.Map<ConversationDto>(conversation);

            _logger.LogInformation("JoinChatRoom: Joined Context Connection id: {ConnectionId}", Context.ConnectionId);

            if (!_connections.Contains(conversation.Id))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString());
            }

            await Clients.Group(conversation.Id.ToString()).SendAsync("JoinChatRoom", conversationDto,
                $"{user.FullName} has joined  conversation {conversation.Id}");
        }

        _logger.LogInformation("END: JoinChatRoom");
    }

    public async Task SendMessage(SendMessageDto request)
    {
        _logger.LogInformation("BEGIN: SendMessage");

        Conversation conversation = await _unitOfWork.Conversations.GetByIdAsync(request.ConversationId);
        if (conversation == null)
        {
            throw new NotFoundException("Conversation does not exist");
        }

        User user = await _userManager.FindByIdAsync(request.AuthorId.ToString());
        if (user == null)
        {
            throw new NotFoundException("User does not exist");
        }

        var message = new Message
        {
            AuthorId = request.AuthorId,
            ConversationId = request.ConversationId,
            Content = request.Content,
            EventId = conversation.EventId,
            VideoUrl = request.VideoUrl,
            VideoFileName = request.VideoFileName,
            ImageUrl = request.ImageUrl,
            ImageFileName = request.ImageFileName,
            AudioUrl = request.AudioUrl,
            AudioFileName = request.AudioFileName,
        };

        await _unitOfWork.Messages.CreateAsync(message);
        await _unitOfWork.CommitAsync();

        MessageDto messageDto = _mapper.Map<MessageDto>(message);

        _logger.LogInformation("SendMessage: Context Connection id: {ConnectionId}", Context.ConnectionId);

        if (!_connections.Contains(conversation.Id))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString());
        }

        await Clients.Group(request.ConversationId.ToString()).SendAsync("ReceivedMessage", messageDto);

        _logger.LogInformation("END: SendMessage");
    }
}

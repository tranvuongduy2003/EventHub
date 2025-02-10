using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Application.SeedWork.DTOs.Message;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.Aggregates.ConversationAggregate.Entities;
using EventHub.Domain.Aggregates.UserAggregate;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Hubs;

public class ChatHub : Hub
{
    private readonly ILogger<ChatHub> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public ChatHub(ILogger<ChatHub> logger, IUnitOfWork unitOfWork, UserManager<User> userManager, IMapper mapper)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("BEGIN: OnConnectedAsync - ConnectionId: {ConnectionId}", Context.ConnectionId);

        await base.OnConnectedAsync();

        _logger.LogInformation("END: OnConnectedAsync");
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("BEGIN: OnDisconnectedAsync - ConnectionId: {ConnectionId}", Context.ConnectionId);

        if (exception != null)
        {
            _logger.LogError(exception, "Error during disconnection for ConnectionId: {ConnectionId}", Context.ConnectionId);
        }

        await base.OnDisconnectedAsync(exception);

        _logger.LogInformation("END: OnDisconnectedAsync");
    }

    public async Task JoinChatRoom(JoinChatRoomDto request)
    {
        _logger.LogInformation("BEGIN: JoinChatRoom - UserId: {UserId}, HostId: {HostId}, EventId: {EventId}",
            request.UserId, request.HostId, request.EventId);

        try
        {
            Domain.Aggregates.EventAggregate.Event @event = await _unitOfWork.Events.GetByIdAsync(request.EventId);
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
                .FindByCondition(x => x.HostId == request.HostId && x.EventId == request.EventId && x.UserId == request.UserId)
                .Include(x => x.Event)
                .Include(x => x.Host)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            if (conversation == null)
            {
                conversation = new Conversation
                {
                    EventId = request.EventId,
                    UserId = request.UserId,
                    HostId = request.HostId,
                };

                await _unitOfWork.Conversations.CreateAsync(conversation);
                await _unitOfWork.CommitAsync();
            }

            ConversationDto conversationDto = _mapper.Map<ConversationDto>(conversation);

            await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString());

            await Clients.Group(conversation.Id.ToString()).SendAsync("JoinChatRoom", conversationDto,
                $"{user.FullName} has joined the conversation.");

            _logger.LogInformation("JoinChatRoom: User {UserId} joined conversation {ConversationId}", request.UserId, conversation.Id);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Resource not found during JoinChatRoom");
            throw;
        }
        catch (BadRequestException ex)
        {
            _logger.LogError(ex, "Bad request during JoinChatRoom");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during JoinChatRoom");
            throw;
        }

        _logger.LogInformation("END: JoinChatRoom");
    }

    public async Task SendMessage(SendMessageDto request)
    {
        _logger.LogInformation("BEGIN: SendMessage - ConversationId: {ConversationId}, SenderId: {SenderId}",
            request.ConversationId, request.SenderId);

        try
        {
            Conversation conversation = await _unitOfWork.Conversations.GetByIdAsync(request.ConversationId);
            if (conversation == null)
            {
                throw new NotFoundException("Conversation does not exist");
            }

            User sender = await _userManager.FindByIdAsync(request.SenderId.ToString());
            if (sender == null)
            {
                throw new NotFoundException("Sender does not exist");
            }

            User receiver = await _userManager.FindByIdAsync(request.ReceiverId.ToString());
            if (receiver == null)
            {
                throw new NotFoundException("Receiver does not exist");
            }

            var message = new Message
            {
                AuthorId = request.SenderId,
                ReceiverId = request.ReceiverId,
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

            message.Author = sender;
            message.Receiver = receiver;
            MessageDto messageDto = _mapper.Map<MessageDto>(message);

            await Clients.Group(request.ConversationId.ToString()).SendAsync("MessageReceived", messageDto);

            _logger.LogInformation("SendMessage: Message sent in conversation {ConversationId}", request.ConversationId);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Resource not found during SendMessage");
            throw;
        }
        catch (BadRequestException ex)
        {
            _logger.LogError(ex, "Bad request during SendMessage");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during SendMessage");
            throw;
        }

        _logger.LogInformation("END: SendMessage");
    }

    public async Task DeleteMessage(DeleteMessageDto request)
    {
        _logger.LogInformation("BEGIN: DeleteMessage - MessageId: {MessageId}, SenderId: {SenderId}",
            request.MessageId, request.SenderId);

        try
        {
            // Tìm tin nhắn theo ID
            Message message = await _unitOfWork.Messages.GetByIdAsync(request.MessageId);
            if (message == null)
            {
                throw new NotFoundException("Message does not exist");
            }

            // Kiểm tra quyền truy cập: Chỉ người tạo tin nhắn mới được phép xóa
            if (message.AuthorId != request.SenderId)
            {
                throw new BadRequestException("You are not authorized to delete this message");
            }

            // Xóa tin nhắn
            await _unitOfWork.Messages.Delete(message);
            await _unitOfWork.CommitAsync();

            // Thông báo cho nhóm rằng tin nhắn đã bị xóa
            await Clients.Group(message.ConversationId.ToString()).SendAsync("MessageDeleted", message.Id);
            _logger.LogInformation("DeleteMessage: Message {MessageId} deleted", request.MessageId);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Resource not found during DeleteMessage");
            throw;
        }
        catch (BadRequestException ex)
        {
            _logger.LogError(ex, "Bad request during DeleteMessage");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during DeleteMessage");
            throw;
        }

        _logger.LogInformation("END: DeleteMessage");
    }

    public async Task EditMessage(EditMessageDto request)
    {
        _logger.LogInformation("BEGIN: EditMessage - MessageId: {MessageId}, SenderId: {SenderId}",
            request.MessageId, request.SenderId);

        try
        {
            // Tìm tin nhắn theo ID
            Message message = await _unitOfWork.Messages.GetByIdAsync(request.MessageId);
            if (message == null)
            {
                throw new NotFoundException("Message does not exist");
            }

            // Kiểm tra quyền truy cập: Chỉ người tạo tin nhắn mới được phép sửa
            if (message.AuthorId != request.SenderId)
            {
                throw new BadRequestException("You are not authorized to edit this message");
            }

            // Cập nhật nội dung tin nhắn
            if (!string.IsNullOrEmpty(request.Content))
            {
                message.Content = request.Content;
            }
            else if (!string.IsNullOrEmpty(request.VideoUrl) && !string.IsNullOrEmpty(request.VideoFileName))
            {
                message.VideoUrl = request.VideoUrl;
                message.VideoFileName = request.VideoFileName;
            }
            else if (!string.IsNullOrEmpty(request.ImageUrl) && !string.IsNullOrEmpty(request.ImageFileName))
            {
                message.ImageUrl = request.ImageUrl;
                message.ImageFileName = request.ImageFileName;
            }
            else if (!string.IsNullOrEmpty(request.AudioUrl) && !string.IsNullOrEmpty(request.AudioFileName))
            {
                message.AudioUrl = request.AudioUrl;
                message.AudioFileName = request.AudioFileName;
            }

            // Lưu thay đổi
            await _unitOfWork.Messages.Update(message);
            await _unitOfWork.CommitAsync();

            // Ánh xạ sang DTO và gửi thông báo về nhóm
            MessageDto updatedMessageDto = _mapper.Map<MessageDto>(message);
            await Clients.Group(message.ConversationId.ToString()).SendAsync("MessageEdited", updatedMessageDto);

            _logger.LogInformation("EditMessage: Message {MessageId} edited", request.MessageId);
        }
        catch (NotFoundException ex)
        {
            _logger.LogError(ex, "Resource not found during EditMessage");
            throw;
        }
        catch (BadRequestException ex)
        {
            _logger.LogError(ex, "Bad request during EditMessage");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during EditMessage");
            throw;
        }

        _logger.LogInformation("END: EditMessage");
    }
}

using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Message;
using EventHub.Shared.Exceptions;
using EventHub.Shared.Helpers;
using EventHub.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Conversation.GetMessagesByConversationId;

public class
    GetMessagesByConversationIdQueryHandler : IQueryHandler<GetMessagesByConversationIdQuery, Pagination<MessageDto>>
{
    private readonly ILogger<GetMessagesByConversationIdQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetMessagesByConversationIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetMessagesByConversationIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Pagination<MessageDto>> Handle(GetMessagesByConversationIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetMessagesByConversationIdQueryHandler");

        var isConversationExisted =
            await _unitOfWork.Conversations.ExistAsync(x => x.Id.Equals(request.ConversationId));
        if (!isConversationExisted)
            throw new NotFoundException("Conversation does not exist!");

        var messages = _unitOfWork.Messages
            .FindByCondition(x => x.ConversationId.Equals(request.ConversationId))
            .Include(x => x.Author);

        var messageDtos = _mapper.Map<List<MessageDto>>(messages);

        _logger.LogInformation("END: GetMessagesByConversationIdQueryHandler");

        return PagingHelper.Paginate<MessageDto>(messageDtos, request.Filter);
    }
}
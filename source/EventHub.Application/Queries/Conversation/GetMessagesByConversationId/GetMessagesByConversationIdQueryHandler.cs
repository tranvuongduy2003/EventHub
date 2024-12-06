using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Message;
using EventHub.Application.Exceptions;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Conversation.GetMessagesByConversationId;

public class
    GetMessagesByConversationIdQueryHandler : IQueryHandler<GetMessagesByConversationIdQuery, Pagination<MessageDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetMessagesByConversationIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<MessageDto>> Handle(GetMessagesByConversationIdQuery request,
        CancellationToken cancellationToken)
    {

        bool isConversationExisted =
            await _unitOfWork.Conversations.ExistAsync(x => x.Id.Equals(request.ConversationId));
        if (!isConversationExisted)
        {
            throw new NotFoundException("Conversation does not exist!");
        }

        IIncludableQueryable<Message, Domain.Aggregates.UserAggregate.User> messages = _unitOfWork.Messages
            .FindByCondition(x => x.ConversationId.Equals(request.ConversationId))
            .Include(x => x.Author);

        List<MessageDto> messageDtos = _mapper.Map<List<MessageDto>>(messages);

        return PagingHelper.Paginate<MessageDto>(messageDtos, request.Filter);
    }
}

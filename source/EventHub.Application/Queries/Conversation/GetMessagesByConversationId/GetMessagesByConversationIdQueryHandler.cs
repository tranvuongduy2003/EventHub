using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Message;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.ConversationAggregate;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EventHub.Application.Queries.Conversation.GetMessagesByConversationId;

public class
    GetMessagesByConversationIdQueryHandler : IQueryHandler<GetMessagesByConversationIdQuery, Pagination<MessageDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetMessagesByConversationIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<MessageDto>> Handle(GetMessagesByConversationIdQuery request,
        CancellationToken cancellationToken)
    {

        bool isConversationExisted =
            await _unitOfWork.Conversations.ExistAsync(x => x.Id == request.ConversationId);
        if (!isConversationExisted)
        {
            throw new NotFoundException("Conversation does not exist!");
        }

        IIncludableQueryable<Message, Domain.Aggregates.UserAggregate.User> messages = _unitOfWork.Messages
            .FindByCondition(x => x.ConversationId == request.ConversationId)
            .Include(x => x.Author);

        List<MessageDto> messageDtos = _mapper.Map<List<MessageDto>>(messages);

        return PagingHelper.Paginate<MessageDto>(messageDtos, request.Filter);
    }
}

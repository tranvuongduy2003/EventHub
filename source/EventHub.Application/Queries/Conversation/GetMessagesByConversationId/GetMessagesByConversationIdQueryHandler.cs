using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Message;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.ConversationAggregate.Entities;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

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

        Pagination<Message> paginatedMessages = _unitOfWork.Messages
            .PaginatedFindByCondition(x => x.ConversationId == request.ConversationId, request.Filter, query => query
                .Include(x => x.Author)
                .Include(x => x.Receiver)
            );

        Pagination<MessageDto> paginatedMessageDtos = _mapper.Map<Pagination<MessageDto>>(paginatedMessages);

        return paginatedMessageDtos;
    }
}

using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Conversation;
using EventHub.Domain.Aggregates.ConversationAggregate.Entities;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EventHub.Application.Queries.Conversation.GetConversationsByOrganizerId;

public class GetConversationsByOrganizerIdQueryHandler : IQueryHandler<GetConversationsByOrganizerIdQuery, Pagination<ConversationDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetConversationsByOrganizerIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<Pagination<ConversationDto>> Handle(GetConversationsByOrganizerIdQuery request,
        CancellationToken cancellationToken)
    {
        IIncludableQueryable<Message, Domain.Aggregates.UserAggregate.User> messages = _unitOfWork.Messages
            .FindAll()
            .Include(x => x.Author);

        Pagination<Domain.Aggregates.ConversationAggregate.Conversation> paginatedConversations = _unitOfWork
            .Conversations
            .PaginatedFindByCondition(
                x => x.HostId == request.OrganizerId,
                request.Filter,
                query => query
                    .Include(x => x.Event)
                    .Include(x => x.Host)
                    .Include(x => x.User)
            );

        paginatedConversations.Items = (
                from _conversation in paginatedConversations.Items
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
            _mapper.Map<Pagination<ConversationDto>>(paginatedConversations);

        return Task.FromResult(paginatedConversationDtos);
    }
}

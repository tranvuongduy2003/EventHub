﻿using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Event.GetEventById;

public class GetEventByIdQueryHandler : IQueryHandler<GetEventByIdQuery, EventDetailDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetEventByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<EventDetailDto> Handle(GetEventByIdQuery request,
        CancellationToken cancellationToken)
    {

        Domain.Aggregates.EventAggregate.Event cachedEvent = await _unitOfWork.CachedEvents
            .FindByCondition(x => x.Id == request.EventId)
            .Include(x => x.EmailContent)
                .ThenInclude(x => x != null ? x.EmailAttachments : default)
            .Include(x => x.EventCategories)
                .ThenInclude(x => x.Category)
            .Include(x => x.Author)
            .Include(x => x.Reasons)
            .Include(x => x.TicketTypes)
            .Include(x => x.EventSubImages)
            .FirstOrDefaultAsync(cancellationToken);

        if (cachedEvent == null)
        {
            throw new NotFoundException("Event does not exist!");
        }

        EventDetailDto eventDto = _mapper.Map<EventDetailDto>(cachedEvent);

        return eventDto;
    }
}

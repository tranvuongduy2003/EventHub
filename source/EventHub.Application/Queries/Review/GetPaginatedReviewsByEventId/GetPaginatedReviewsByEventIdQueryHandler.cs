﻿using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.Shared.Helpers;
using EventHub.Domain.Shared.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Review.GetPaginatedReviewsByEventId;

public class
    GetPaginatedReviewsByEventIdQueryHandler : IQueryHandler<GetPaginatedReviewsByEventIdQuery, Pagination<ReviewDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetPaginatedReviewsByEventIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Pagination<ReviewDto>> Handle(GetPaginatedReviewsByEventIdQuery request,
        CancellationToken cancellationToken)
    {
        bool isEventExisted = await _unitOfWork.Events.ExistAsync(x => x.Id == request.EventId);
        if (!isEventExisted)
        {
            throw new NotFoundException("Event does not exist!");
        }

        Pagination<Domain.Aggregates.EventAggregate.Entities.Review> paginatedReviews = _unitOfWork.CachedReviews
            .PaginatedFindByCondition(x => x.EventId == request.EventId, request.Filter, query => query
                .Include(x => x.Event)
                .Include(x => x.Author)
            );

        Pagination<ReviewDto> paginatedReviewDtos = _mapper.Map<Pagination<ReviewDto>>(paginatedReviews);

        return paginatedReviewDtos;
    }
}

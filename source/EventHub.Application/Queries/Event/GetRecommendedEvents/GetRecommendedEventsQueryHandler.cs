using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Event;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace EventHub.Application.Queries.Event.GetRecommendedEvents;

public class GetRecommendedEventsQueryHandler : IQueryHandler<GetRecommendedEventsQuery, List<EventDto>>
{
    private readonly IMapper _mapper;
    private readonly SignInManager<Domain.Aggregates.UserAggregate.User> _signInManager;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IUnitOfWork _unitOfWork;

    public GetRecommendedEventsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, SignInManager<Domain.Aggregates.UserAggregate.User> signInManager, IHttpClientFactory clientFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _signInManager = signInManager;
        _clientFactory = clientFactory;
    }

    public async Task<List<EventDto>> Handle(GetRecommendedEventsQuery request,
        CancellationToken cancellationToken)
    {
        string userId = _signInManager.Context.User.Identities.FirstOrDefault()?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? "";

        HttpClient httpClient = _clientFactory.CreateClient("RecommenderSystem");
        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync($"/api/recommendations/{userId}", cancellationToken);
        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            return new List<EventDto>();
        }

        string apiContent = await httpResponseMessage.Content.ReadAsStringAsync(cancellationToken);
        List<RecommenderResponseDto> eventIds = JsonConvert.DeserializeObject<List<RecommenderResponseDto>>(apiContent) ?? new List<RecommenderResponseDto>();

        var events = _unitOfWork.Events
            .FindAll()
            .Join(eventIds.Select(x => x.Id).ToList(),
                _event => _event.Id,
                _eventId => _eventId,
                (_event, _id) => _event)
            .Include(x => x.Reviews)
            .Include(x => x.EventCategories).ThenInclude(x => x.Category)
            .Include(x => x.EventCoupons).ThenInclude(x => x.Coupon)
            .Include(x => x.TicketTypes)
            .Include(x => x.Expenses)
            .ToList();

        List<EventDto> eventDtos = _mapper.Map<List<EventDto>>(events);

        for (int i = 0; i < eventDtos.Count; i++)
        {
            eventDtos[i].AverageRate = events[i].Reviews != null && events[i].Reviews.Any() ? Math.Round(events[i].Reviews.Average(x => x.Rate), 2) : 0.00;
        }

        return eventDtos;
    }
}

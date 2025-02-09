using EventHub.Application.SeedWork.DTOs.Review;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
using Newtonsoft.Json;

namespace EventHub.Application.DomainEventHandlers;

public class CalculatePositivePercentageOfReviewDomainEventHandler : IDomainEventHandler<CalculatePositivePercentageOfReviewDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpClientFactory _clientFactory;

    public CalculatePositivePercentageOfReviewDomainEventHandler(IUnitOfWork unitOfWork, IHttpClientFactory clientFactory)
    {
        _unitOfWork = unitOfWork;
        _clientFactory = clientFactory;
    }

    public async Task Handle(CalculatePositivePercentageOfReviewDomainEvent notification, CancellationToken cancellationToken)
    {
        Domain.Aggregates.EventAggregate.Entities.Review review = await _unitOfWork.Reviews.GetByIdAsync(notification.ReviewId);
        if (review is null)
        {
            return;
        }

        HttpClient client = _clientFactory.CreateClient("SentimentAnalysis");
        HttpResponseMessage httpResponse = await client.GetAsync($"/predict?text=${review.Content}", cancellationToken);
        string apiContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
        SentimentAnalysisResponseDto response = JsonConvert.DeserializeObject<SentimentAnalysisResponseDto>(apiContent);

        review.IsPositive = response?.Result == "Positive";
        review.SentimentPercentage = response is not null ? response.Percentage : 0.0;

        await _unitOfWork.Reviews.Update(review);

        await _unitOfWork.CommitAsync();
    }
}

using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Review;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Review.CreateReview;

public class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, ReviewDto>
{
    private readonly ILogger<CreateReviewCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<CreateReviewCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateEventCommandHandler");

        var review = new Domain.AggregateModels.ReviewAggregate.Review
        {
            Rate = request.Rate,
            AuthorId = request.AuthorId,
            EventId = request.EventId,
            Content = request.Content
        };

        await _unitOfWork.Reviews.CreateAsync(review);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: CreateEventCommandHandler");

        return _mapper.Map<ReviewDto>(review);
    }
}
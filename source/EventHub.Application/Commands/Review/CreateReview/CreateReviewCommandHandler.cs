using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Review;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Review.CreateReview;

public class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, ReviewDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ReviewDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
    {
        var review = new Domain.Aggregates.ReviewAggregate.Review
        {
            Rate = request.Rate,
            AuthorId = request.AuthorId,
            EventId = request.EventId,
            Content = request.Content
        };

        await _unitOfWork.Reviews.CreateAsync(review);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<ReviewDto>(review);
    }
}
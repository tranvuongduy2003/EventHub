using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Review.UpdateReview;

public class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.ReviewAggregate.Review review = await _unitOfWork.Reviews.GetByIdAsync(request.Id);
        if (review is null)
        {
            throw new NotFoundException("Review does not exist!");
        }

        review.Content = request.Content;
        review.Rate = request.Rate;

        await _unitOfWork.Reviews.Update(review);
        await _unitOfWork.CommitAsync();
    }
}

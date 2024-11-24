using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Review.DeleteReview;

public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        Domain.AggregateModels.ReviewAggregate.Review review = await _unitOfWork.Reviews.GetByIdAsync(request.Id);
        if (review is null)
        {
            throw new NotFoundException("Review does not exist!");
        }

        _unitOfWork.Reviews.SoftDelete(review);
        await _unitOfWork.CommitAsync();
    }
}

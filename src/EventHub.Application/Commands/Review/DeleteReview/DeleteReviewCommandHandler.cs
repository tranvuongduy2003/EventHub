using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;

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
        var review = await _unitOfWork.Reviews.GetByIdAsync(request.Id);
        if (review is null)
            throw new NotFoundException("Review does not exist!");

        await _unitOfWork.Reviews.SoftDeleteAsync(review);
        await _unitOfWork.CommitAsync();
    }
}
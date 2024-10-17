using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;

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
        var review = await _unitOfWork.Reviews.GetByIdAsync(request.Id);
        if (review is null)
            throw new NotFoundException("Review does not exist!");

        review.Content = request.Review.Content;
        review.Rate = request.Review.Rate;

        await _unitOfWork.Reviews.UpdateAsync(review);
        await _unitOfWork.CommitAsync();
    }
}
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Review.DeleteReview;

public class DeleteReviewCommandHandler : ICommandHandler<DeleteReviewCommand>
{
    private readonly ILogger<DeleteReviewCommandHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteReviewCommandHandler(IUnitOfWork unitOfWork,
        ILogger<DeleteReviewCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteReviewCommandHandler");

        var review = await _unitOfWork.Reviews.GetByIdAsync(request.Id);
        if (review is null)
            throw new NotFoundException("Review does not exist!");

        await _unitOfWork.Reviews.SoftDeleteAsync(review);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: DeleteReviewCommandHandler");
    }
}
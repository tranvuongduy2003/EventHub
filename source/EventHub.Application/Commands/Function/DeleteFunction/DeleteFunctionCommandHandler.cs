using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;

namespace EventHub.Application.Commands.Function.DeleteFunction;

public class DeleteFunctionCommandHandler : ICommandHandler<DeleteFunctionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFunctionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DeleteFunctionCommand request, CancellationToken cancellationToken)
    {
        Domain.AggregateModels.PermissionAggregate.Function function = await _unitOfWork.Functions.GetByIdAsync(request.FunctionId);
        if (function is null)
        {
            throw new NotFoundException("Function does not exist!");
        }

        await _unitOfWork.Functions.Delete(function);
        await _unitOfWork.CommitAsync();
    }
}

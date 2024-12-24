using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Commands.Permission.DisableCommandInFunction;

public class DisableCommandInFunctionCommandHandler : ICommandHandler<DisableCommandInFunctionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DisableCommandInFunctionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DisableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        bool isFunctionExisted = await _unitOfWork.Functions.ExistAsync(request.FunctionId);
        if (!isFunctionExisted)
        {
            throw new NotFoundException("Function does not exist!");
        }

        bool isCommandExisted = await _unitOfWork.Commands.ExistAsync(request.CommandId);
        if (!isCommandExisted)
        {
            throw new NotFoundException("Command does not exist!");
        }

        CommandInFunction commandInFunction = await _unitOfWork.CommandInFunctions
            .FindByCondition(x =>
                x.FunctionId == request.FunctionId &&
                x.CommandId == request.CommandId)
            .FirstOrDefaultAsync(cancellationToken);
        if (commandInFunction is null)
        {
            throw new NotFoundException("This command is not existed in function.");
        }

        await _unitOfWork.CommandInFunctions.Delete(commandInFunction);
        await _unitOfWork.CommitAsync();
    }
}

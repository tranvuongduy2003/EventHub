using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.DomainEventHandlers;

public class DisableCommandInFunctionDomainEventHandler : IDomainEventHandler<DisableCommandInFunctionDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public DisableCommandInFunctionDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(DisableCommandInFunctionDomainEvent notification, CancellationToken cancellationToken)
    {
        var isFunctionExisted = await _unitOfWork.Functions.ExistAsync(notification.FunctionId);
        if (!isFunctionExisted)
            throw new NotFoundException("Function does not exist!");

        var isCommandExisted = await _unitOfWork.Commands.ExistAsync(notification.CommandId);
        if (!isCommandExisted)
            throw new NotFoundException("Command does not exist!");

        var commandInFunction = await _unitOfWork.CommandInFunctions
            .FindByCondition(x =>
                x.FunctionId.Equals(notification.FunctionId) &&
                x.CommandId.Equals(notification.CommandId))
            .FirstOrDefaultAsync();
        if (commandInFunction is null)
            throw new NotFoundException("This command is not existed in function.");

        await _unitOfWork.CommandInFunctions.DeleteAsync(commandInFunction);
        await _unitOfWork.CommitAsync();
    }
}
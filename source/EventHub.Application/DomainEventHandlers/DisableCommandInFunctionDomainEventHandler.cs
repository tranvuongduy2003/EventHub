using EventHub.Application.Abstractions;
using EventHub.Application.Exceptions;
using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;
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
        bool isFunctionExisted = await _unitOfWork.Functions.ExistAsync(notification.FunctionId);
        if (!isFunctionExisted)
        {
            throw new NotFoundException("Function does not exist!");
        }

        bool isCommandExisted = await _unitOfWork.Commands.ExistAsync(notification.CommandId);
        if (!isCommandExisted)
        {
            throw new NotFoundException("Command does not exist!");
        }

        CommandInFunction commandInFunction = await _unitOfWork.CommandInFunctions
            .FindByCondition(x =>
                x.FunctionId.Equals(notification.FunctionId, StringComparison.Ordinal) &&
                x.CommandId.Equals(notification.CommandId, StringComparison.Ordinal))
            .FirstOrDefaultAsync(cancellationToken);
        if (commandInFunction is null)
        {
            throw new NotFoundException("This command is not existed in function.");
        }

        await _unitOfWork.CommandInFunctions.Delete(commandInFunction);
        await _unitOfWork.CommitAsync();
    }
}

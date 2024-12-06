using EventHub.Application.Abstractions;
using EventHub.Application.Exceptions;
using EventHub.Domain.Aggregates.PermissionAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.DomainEventHandlers;

public class EnableCommandInFunctionDomainEventHandler : IDomainEventHandler<EnableCommandInFunctionDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public EnableCommandInFunctionDomainEventHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EnableCommandInFunctionDomainEvent notification, CancellationToken cancellationToken)
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

        bool isCommandInFunctionExisted = await _unitOfWork.CommandInFunctions.ExistAsync(x =>
            x.FunctionId.Equals(notification.FunctionId, StringComparison.Ordinal) &&
            x.FunctionId.Equals(notification.CommandId, StringComparison.Ordinal));
        if (!isCommandInFunctionExisted)
        {
            throw new BadRequestException("This command has been added to function.");
        }

        var entity = new CommandInFunction()
        {
            CommandId = notification.CommandId,
            FunctionId = notification.FunctionId,
        };

        await _unitOfWork.CommandInFunctions.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }
}

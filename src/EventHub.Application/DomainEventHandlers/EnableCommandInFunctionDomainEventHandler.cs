using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Application.Exceptions;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using Microsoft.Extensions.Logging;

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
        var isFunctionExisted = await _unitOfWork.Functions.ExistAsync(notification.FunctionId);
        if (!isFunctionExisted)
            throw new NotFoundException("Function does not exist!");

        var isCommandExisted = await _unitOfWork.Commands.ExistAsync(notification.CommandId);
        if (!isCommandExisted)
            throw new NotFoundException("Command does not exist!");

        var commandInFunction = await _unitOfWork.CommandInFunctions.ExistAsync(x =>
            x.FunctionId.Equals(notification.FunctionId) &&
            x.FunctionId.Equals(notification.CommandId));
        if (!commandInFunction)
            throw new BadRequestException("This command has been added to function.");

        var entity = new CommandInFunction()
        {
            CommandId = notification.CommandId,
            FunctionId = notification.FunctionId,
        };

        await _unitOfWork.CommandInFunctions.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }
}
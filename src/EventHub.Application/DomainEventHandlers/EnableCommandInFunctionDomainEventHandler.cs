using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class EnableCommandInFunctionDomainEventHandler : IDomainEventHandler<EnableCommandInFunctionDomainEvent>
{
    private readonly ILogger<EnableCommandInFunctionDomainEventHandler> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public EnableCommandInFunctionDomainEventHandler(IUnitOfWork unitOfWork,
        ILogger<EnableCommandInFunctionDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(EnableCommandInFunctionDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: EnableCommandInFunctionDomainEventHandler");

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

        _logger.LogInformation("END: EnableCommandInFunctionDomainEventHandler");
    }
}
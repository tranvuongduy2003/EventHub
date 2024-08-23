using AutoMapper;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.DomainEvent;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class DisableCommandInFunctionDomainEventHandler : IDomainEventHandler<DisableCommandInFunctionDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DisableCommandInFunctionDomainEventHandler> _logger;

    public DisableCommandInFunctionDomainEventHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<DisableCommandInFunctionDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(DisableCommandInFunctionDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DisableCommandInFunctionDomainEventHandler");

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
            .GetAwaiter()
            .GetResult()
            .FirstOrDefaultAsync();
        if (commandInFunction is null)
            throw new NotFoundException("This command is not existed in function.");

        await _unitOfWork.CommandInFunctions.DeleteAsync(commandInFunction);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: DisableCommandInFunctionDomainEventHandler");
    }
}
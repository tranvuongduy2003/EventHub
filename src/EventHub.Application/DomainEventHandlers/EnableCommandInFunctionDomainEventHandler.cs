using AutoMapper;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class EnableCommandInFunctionDomainEventHandler : INotificationHandler<EnableCommandInFunctionDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<EnableCommandInFunctionDomainEventHandler> _logger;
    
    public EnableCommandInFunctionDomainEventHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EnableCommandInFunctionDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(EnableCommandInFunctionDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: EnableCommandInFunctionDomainEventHandler");
        
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
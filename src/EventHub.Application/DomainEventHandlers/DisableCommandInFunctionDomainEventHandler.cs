using AutoMapper;
using EventHub.Domain.Events;
using EventHub.Domain.SeedWork.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.DomainEventHandlers;

public class DisableCommandInFunctionDomainEventHandler : INotificationHandler<DisableCommandInFunctionDomainEvent>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DisableCommandInFunctionDomainEventHandler> _logger;
    
    public DisableCommandInFunctionDomainEventHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DisableCommandInFunctionDomainEventHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(DisableCommandInFunctionDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DisableCommandInFunctionDomainEventHandler");


        _logger.LogInformation("END: DisableCommandInFunctionDomainEventHandler");
    }
}
using AutoMapper;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Function;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Function.CreateFunction;

public class CreateFunctionCommandHandler : IRequestHandler<CreateFunctionCommand, FunctionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateFunctionCommandHandler> _logger;

    public CreateFunctionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateFunctionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<FunctionDto> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateFunctionCommandHandler");

        var function = _mapper.Map<Domain.AggregateModels.PermissionAggregate.Function>(request);

        await _unitOfWork.Functions.CreateAsync(function);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("END: CreateFunctionCommandHandler");
        
        return _mapper.Map<FunctionDto>(function);
    }
}
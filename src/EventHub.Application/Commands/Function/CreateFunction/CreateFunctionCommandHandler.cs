using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Function;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Function.CreateFunction;

public class CreateFunctionCommandHandler : ICommandHandler<CreateFunctionCommand, FunctionDto>
{
    private readonly ILogger<CreateFunctionCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFunctionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<CreateFunctionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<FunctionDto> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: CreateFunctionCommandHandler");

        var function = new Domain.AggregateModels.PermissionAggregate.Function()
        {
            Name = request.Name,
            Url = request.Url,
            SortOrder = request.SortOrder,
            ParentId = request.ParentId
        };

        await _unitOfWork.Functions.CreateAsync(function);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: CreateFunctionCommandHandler");

        return _mapper.Map<FunctionDto>(function);
    }
}
using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Function.UpdateFunction;

public class UpdateFunctionCommandHandler : ICommandHandler<UpdateFunctionCommand>
{
    private readonly ILogger<UpdateFunctionCommandHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFunctionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper,
        ILogger<UpdateFunctionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task Handle(UpdateFunctionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: UpdateFunctionCommandHandler");

        var function = await _unitOfWork.Functions.GetByIdAsync(request.Id);
        if (function is null)
            throw new NotFoundException("Function does not exist!");

        function = _mapper.Map<Domain.AggregateModels.PermissionAggregate.Function>(request.Function);

        await _unitOfWork.Functions.UpdateAsync(function);
        await _unitOfWork.CommitAsync();

        _logger.LogInformation("END: UpdateFunctionCommandHandler");
    }
}
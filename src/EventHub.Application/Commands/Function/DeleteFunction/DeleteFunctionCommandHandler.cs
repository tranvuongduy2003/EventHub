using AutoMapper;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.Exceptions;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Commands.Function.DeleteFunction;

public class DeleteFunctionCommandHandler : ICommandHandler<DeleteFunctionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteFunctionCommandHandler> _logger;

    public DeleteFunctionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DeleteFunctionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task Handle(DeleteFunctionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: DeleteFunctionCommandHandler");

        var function = await _unitOfWork.Functions.GetByIdAsync(request.FunctionId);
        if (function is null)
            throw new NotFoundException("Function does not exist!");

        await _unitOfWork.Functions.DeleteAsync(function);
        await _unitOfWork.CommitAsync();
        
        _logger.LogInformation("END: DeleteFunctionCommandHandler");
    }
}
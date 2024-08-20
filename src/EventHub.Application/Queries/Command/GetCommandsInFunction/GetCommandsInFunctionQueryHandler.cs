using AutoMapper;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Command;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Command.GetCommandsInFunction;

public class GetCommandsInFunctionQueryHandler : IRequestHandler<GetCommandsInFunctionQuery, List<CommandDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCommandsInFunctionQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetCommandsInFunctionQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetCommandsInFunctionQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<CommandDto>> Handle(GetCommandsInFunctionQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetCommandsInFunctionQueryHandler");

        var commands = await _unitOfWork.CommandInFunctions
            .FindByCondition(x => x.FunctionId.Equals(request.FunctionId))
            .DistinctBy(x => x.CommandId)
            .Include(x => x.Command)
            .Select(x => x.Command)
            .ToListAsync();
        
        _logger.LogInformation("END: GetCommandsInFunctionQueryHandler");
        
        return _mapper.Map<List<CommandDto>>(commands);
    }
}
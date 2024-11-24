using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Command;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Command.GetCommandsInFunction;

public class GetCommandsInFunctionQueryHandler : IQueryHandler<GetCommandsInFunctionQuery, List<CommandDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCommandsInFunctionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CommandDto>> Handle(GetCommandsInFunctionQuery request,
        CancellationToken cancellationToken)
    {

        List<Domain.AggregateModels.PermissionAggregate.Command> commands = await _unitOfWork.CommandInFunctions
            .FindByCondition(x => x.FunctionId.Equals(request.FunctionId, StringComparison.Ordinal), includeProperties: x => x.Command)
            .DistinctBy(x => x.CommandId)
            .Select(x => x.Command)
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<CommandDto>>(commands);
    }
}

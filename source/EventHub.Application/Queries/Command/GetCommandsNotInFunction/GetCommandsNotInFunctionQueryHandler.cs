using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.AggregateModels.PermissionAggregate;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Command;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Command.GetCommandsNotInFunction;

public class GetCommandsNotInFunctionQueryHandler : IQueryHandler<GetCommandsNotInFunctionQuery, List<CommandDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCommandsNotInFunctionQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CommandDto>> Handle(GetCommandsNotInFunctionQuery request,
        CancellationToken cancellationToken)
    {
        List<CommandInFunction> commandInFunctions = await _unitOfWork.CommandInFunctions
           .FindByCondition(x => x.FunctionId == request.FunctionId)
           .ToListAsync(cancellationToken);

        var commands = _unitOfWork.Commands
            .FindAll()
            .AsEnumerable()
            .Where(x => !commandInFunctions.Any(cif => cif.CommandId == x.Id))
            .ToList();

        return _mapper.Map<List<CommandDto>>(commands);
    }
}

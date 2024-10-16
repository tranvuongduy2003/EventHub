using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Command;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Command.GetCommandsNotInFunction;

public class GetCommandsNotInFunctionQueryHandler : IQueryHandler<GetCommandsNotInFunctionQuery, List<CommandDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetCommandsNotInFunctionQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<CommandDto>> Handle(GetCommandsNotInFunctionQuery request,
        CancellationToken cancellationToken)
    {


        var commandInFunctions = _unitOfWork.CommandInFunctions
            .FindByCondition(x => x.FunctionId.Equals(request.FunctionId));

        var commands = _unitOfWork.Commands
            .FindByCondition(x => !commandInFunctions.Any(cif => cif.CommandId == x.Id))
            .ToListAsync();


        return _mapper.Map<List<CommandDto>>(commands);
    }
}
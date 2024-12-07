using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Command;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;

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

    public Task<List<CommandDto>> Handle(GetCommandsInFunctionQuery request,
        CancellationToken cancellationToken)
    {
        var commands = _unitOfWork.CommandInFunctions
            .FindByCondition(x => x.FunctionId == request.FunctionId, includeProperties: x => x.Command)
            .AsEnumerable()
            .DistinctBy(x => x.CommandId)
            .Select(x => x.Command)
            .ToList();

        return Task.FromResult(_mapper.Map<List<CommandDto>>(commands));
    }
}

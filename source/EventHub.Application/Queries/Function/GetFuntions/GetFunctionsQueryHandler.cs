using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Function;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Function.GetFuntions;

public class GetFunctionsQueryHandler : IQueryHandler<GetFunctionsQuery, List<FunctionDto>>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetFunctionsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<FunctionDto>> Handle(GetFunctionsQuery request,
        CancellationToken cancellationToken)
    {
        List<Domain.AggregateModels.PermissionAggregate.Function> functions = await _unitOfWork.Functions
            .FindAll()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<FunctionDto>>(functions);
    }
}

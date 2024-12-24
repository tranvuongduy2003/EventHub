using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Function;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.EntityFrameworkCore;

namespace EventHub.Application.Queries.Function.GetFunctions;

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
        List<Domain.Aggregates.UserAggregate.Entities.Function> functions = await _unitOfWork.Functions
            .FindAll()
            .ToListAsync(cancellationToken);

        return _mapper.Map<List<FunctionDto>>(functions);
    }
}

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
            .FindByCondition(x => x.ParentId == null)
            .ToListAsync(cancellationToken);

        List<FunctionDto> functionDtos = _mapper.Map<List<FunctionDto>>(functions);

        foreach (FunctionDto function in functionDtos)
        {
            List<Domain.Aggregates.UserAggregate.Entities.Function> children = await _unitOfWork.Functions
                .FindByCondition(x => x.ParentId == function.Id)
                .ToListAsync(cancellationToken);

            function.Children = _mapper.Map<List<FunctionDto>>(children);
        }

        return functionDtos;
    }
}

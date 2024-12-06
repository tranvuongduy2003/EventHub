using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.DTOs.Function;
using EventHub.Domain.SeedWork.Persistence;
using EventHub.Domain.SeedWork.Query;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Function.GetFunctionById;

public class GetFunctionByIdQueryHandler : IQueryHandler<GetFunctionByIdQuery, FunctionDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GetFunctionByIdQueryHandler(IUnitOfWork unitOfWork,IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FunctionDto> Handle(GetFunctionByIdQuery request,
        CancellationToken cancellationToken)
    {

        Domain.Aggregates.PermissionAggregate.Function function = await _unitOfWork.Functions.GetByIdAsync(request.Id);
        
        return _mapper.Map<FunctionDto>(function);
    }
}

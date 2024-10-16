using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Function;
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

        var function = await _unitOfWork.Functions.GetByIdAsync(request.Id);


        return _mapper.Map<FunctionDto>(function);
    }
}
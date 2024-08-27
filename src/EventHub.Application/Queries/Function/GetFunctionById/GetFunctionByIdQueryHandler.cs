using AutoMapper;
using EventHub.Domain.SeedWork.Query;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Function;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Function.GetFunctionById;

public class GetFunctionByIdQueryHandler : IQueryHandler<GetFunctionByIdQuery, FunctionDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFunctionByIdQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetFunctionByIdQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetFunctionByIdQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<FunctionDto> Handle(GetFunctionByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetFunctionByIdQueryHandler");

        var function = await _unitOfWork.Functions.GetByIdAsync(request.Id);
        
        _logger.LogInformation("END: GetFunctionByIdQueryHandler");
        
        return _mapper.Map<FunctionDto>(function);
    }
}
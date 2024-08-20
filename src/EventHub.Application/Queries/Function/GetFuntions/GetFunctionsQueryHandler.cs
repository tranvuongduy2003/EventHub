using AutoMapper;
using EventHub.Domain.SeedWork.UnitOfWork;
using EventHub.Shared.DTOs.Function;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Function.GetFuntions;

public class GetFunctionsQueryHandler : IRequestHandler<GetFunctionsQuery, List<FunctionDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetFunctionsQueryHandler> _logger;
    private readonly IMapper _mapper;

    public GetFunctionsQueryHandler(IUnitOfWork unitOfWork,
        ILogger<GetFunctionsQueryHandler> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<FunctionDto>> Handle(GetFunctionsQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("BEGIN: GetFunctionsQueryHandler");

        var functions = _unitOfWork.Functions.FindAll();
        
        _logger.LogInformation("END: GetFunctionsQueryHandler");
        
        return _mapper.Map<List<FunctionDto>>(functions);
    }
}
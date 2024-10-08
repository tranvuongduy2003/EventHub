using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Query;
using EventHub.Shared.DTOs.Function;
using Microsoft.Extensions.Logging;

namespace EventHub.Application.Queries.Function.GetFuntions;

public class GetFunctionsQueryHandler : IQueryHandler<GetFunctionsQuery, List<FunctionDto>>
{
    private readonly ILogger<GetFunctionsQueryHandler> _logger;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

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
using AutoMapper;
using EventHub.Abstractions.SeedWork.UnitOfWork;
using EventHub.Domain.SeedWork.Command;
using EventHub.Shared.DTOs.Function;

namespace EventHub.Application.Commands.Function.CreateFunction;

public class CreateFunctionCommandHandler : ICommandHandler<CreateFunctionCommand, FunctionDto>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFunctionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FunctionDto> Handle(CreateFunctionCommand request, CancellationToken cancellationToken)
    {
        var function = new Domain.AggregateModels.PermissionAggregate.Function()
        {
            Name = request.Name,
            Url = request.Url,
            SortOrder = request.SortOrder,
            ParentId = request.ParentId
        };

        await _unitOfWork.Functions.CreateAsync(function);
        await _unitOfWork.CommitAsync();

        return _mapper.Map<FunctionDto>(function);
    }
}
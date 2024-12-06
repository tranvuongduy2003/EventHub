using AutoMapper;
using EventHub.Application.Abstractions;
using EventHub.Application.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Function.UpdateFunction;

public class UpdateFunctionCommandHandler : ICommandHandler<UpdateFunctionCommand>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFunctionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateFunctionCommand request, CancellationToken cancellationToken)
    {
        Domain.Aggregates.PermissionAggregate.Function function = await _unitOfWork.Functions.GetByIdAsync(request.Id);
        if (function is null)
        {
            throw new NotFoundException("Function does not exist!");
        }

        function = _mapper.Map<Domain.Aggregates.PermissionAggregate.Function>(request);

        await _unitOfWork.Functions.Update(function);
        await _unitOfWork.CommitAsync();
    }
}

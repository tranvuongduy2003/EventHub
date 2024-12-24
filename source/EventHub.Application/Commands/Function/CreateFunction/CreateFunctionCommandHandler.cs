using AutoMapper;
using EventHub.Application.SeedWork.DTOs.Function;
using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

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
        bool isParentFucntionExisted = await _unitOfWork.Functions.ExistAsync(x => x.Id == request.ParentId);

        if (!isParentFucntionExisted)
        {
            throw new NotFoundException("ParentId does not exist!");
        }

        var function = new Domain.Aggregates.UserAggregate.Entities.Function()
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

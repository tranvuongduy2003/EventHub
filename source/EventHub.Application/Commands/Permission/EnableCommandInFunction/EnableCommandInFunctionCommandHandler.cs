using EventHub.Application.SeedWork.Exceptions;
using EventHub.Domain.Aggregates.UserAggregate.ValueObjects;
using EventHub.Domain.SeedWork.Command;
using EventHub.Domain.SeedWork.Persistence;

namespace EventHub.Application.Commands.Permission.EnableCommandInFunction;

public class EnableCommandInFunctionCommandHandler : ICommandHandler<EnableCommandInFunctionCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public EnableCommandInFunctionCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(EnableCommandInFunctionCommand request, CancellationToken cancellationToken)
    {
        bool isFunctionExisted = await _unitOfWork.Functions.ExistAsync(request.FunctionId);
        if (!isFunctionExisted)
        {
            throw new NotFoundException("Function does not exist!");
        }

        bool isCommandExisted = await _unitOfWork.Commands.ExistAsync(request.CommandId);
        if (!isCommandExisted)
        {
            throw new NotFoundException("Command does not exist!");
        }

        bool isCommandInFunctionExisted = await _unitOfWork.CommandInFunctions.ExistAsync(x =>
            x.FunctionId == request.FunctionId &&
            x.CommandId == request.CommandId);
        if (!isCommandInFunctionExisted)
        {
            throw new BadRequestException("This command has been added to function.");
        }

        var entity = new CommandInFunction()
        {
            CommandId = request.CommandId,
            FunctionId = request.FunctionId,
        };

        await _unitOfWork.CommandInFunctions.CreateAsync(entity);
        await _unitOfWork.CommitAsync();
    }
}
